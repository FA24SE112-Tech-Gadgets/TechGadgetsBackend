using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Reviews.Mappers;
using WebApi.Features.Reviews.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Reviews;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class GetGadgetsReviews : ControllerBase
{
    public new class Request : PagedRequest
    {
        public SortByDate? SortByDate { get; set; }
        public FilterBy FilterBy { get; set; }
    }

    public enum SortByDate
    {
        DESC, ASC
    }

    public enum FilterBy
    {
        NotReview, Reviewed, NotReply, Replied
    }

    [HttpGet("reviews/seller-order-items")]
    [Tags("Reviews")]
    [SwaggerOperation(
        Summary = "Customer/Seller Get Their List Gadgets Need Review/Reviewed/Need Reply/Replied",
        Description = "API is for get list of gadgets need review/reviewed/need reply/replied base on Customer/Seller. Note:" +
                            "<br>&nbsp; - FilterBy: 'NotReview', 'Reviewed', 'NotReply', 'Replied'. Default: 'NotReview'" +
                            "<br>&nbsp; - Dùng API này để lấy ra danh sách các sản phầm chưa đánh giá/đã đánh giá (Customer) và chưa phản hồi/đã phản hồi (Seller)." +
                            "<br>&nbsp; - Customer: NotReview, Reviewed." +
                            "<br>&nbsp; - Seller: NotReply, Replied." +
                            "<br>&nbsp; - Sử dụng sellerOrderItemId để đánh giá, không phải gadgetId." +
                            "<br>&nbsp; - Sử dụng gadgetId để click vô xem gadget detail." +
                            "<br>&nbsp; - Nếu Customer gọi API này thì sẽ thấy những item có review status = Inactive, còn Seller thì không thấy những item đó luôn." +
                            "<br>&nbsp;     Tại vì review bị chặn thì seller cũng không cần phải phản hồi hay chưa phản hồi. Và gadget detail thì vẫn có thể xem ở chỗ khác được, không cần phải xem ở trang review"
    )]
    [ProducesResponseType(typeof(PagedList<GadgetReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Role == Role.Customer)
        {
            var query = context.Orders
            .Include(o => o.SellerOrders)
            .Where(o => o.CustomerId == currentUser!.Customer!.Id)
            .SelectMany(o => o.SellerOrders)
            .Where(so => so.Status == SellerOrderStatus.Success)
            .Include(so => so.SellerOrderItems)
                .ThenInclude(soi => soi.Gadget.Category)
            .Include(so => so.SellerOrderItems)
                .ThenInclude(soi => soi.Review)
                .ThenInclude(r => r != null ? r.Customer : null)
            .Include(so => so.SellerOrderItems)
                    .ThenInclude(soi => soi.Review)
                        .ThenInclude(r => r != null ? r.SellerReply : null)
                        .ThenInclude(sr => sr != null ? sr.Seller : null)
            .AsQueryable();

            if (request.FilterBy != FilterBy.Reviewed && request.FilterBy != FilterBy.NotReview)
            {
                throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEA_00)
                .AddReason("filterBy", "Customer dùng sai filterBy.")
                .Build();
            }

            // Sort theo SortByDate
            if (request.SortByDate != null)
            {
                query = request.SortByDate == SortByDate.ASC
                    ? query.OrderBy(od => od.CreatedAt)
                    : query.OrderByDescending(od => od.CreatedAt);
            }

            var sellerOrders = await query.ToListAsync();

            List<GadgetReviewResponse> gadgetReviewResponses = new List<GadgetReviewResponse>()!;
            foreach (var so in sellerOrders)
            {
                var sellerOrderItems = so.SellerOrderItems;
                foreach (var soi in sellerOrderItems)
                {
                    bool isReviewed = soi.Review != null;
                    if (request.FilterBy == FilterBy.Reviewed)
                    {
                        if (isReviewed)
                        {
                            GadgetReviewResponse gadgetReviewResponse = new GadgetReviewResponse()
                            {
                                SellerOrderItemId = soi.Id,
                                GadgetId = soi.GadgetId,
                                Name = soi.Gadget.Name,
                                ThumbnailUrl = soi.Gadget.ThumbnailUrl,
                                Review = soi.Review!.ToReviewResponse(),
                                Status = soi.Gadget.Status,
                            }!;
                            gadgetReviewResponses.Add(gadgetReviewResponse);
                        }
                    }
                    else
                    {
                        if (!isReviewed && so.UpdatedAt > DateTime.UtcNow.AddMinutes(-10))
                        {
                            GadgetReviewResponse gadgetReviewResponse = new GadgetReviewResponse()
                            {
                                SellerOrderItemId = soi.Id,
                                GadgetId = soi.GadgetId,
                                Name = soi.Gadget.Name,
                                ThumbnailUrl = soi.Gadget.ThumbnailUrl,
                                Status = soi.Gadget.Status,
                            }!;
                            gadgetReviewResponses.Add(gadgetReviewResponse);
                        }
                    }
                }
            }
            int page = request.Page == null ? 1 : (int)request.Page;
            int pageSize = request.PageSize == null ? 10 : (int)request.PageSize;
            int skip = (page - 1) * pageSize;

            var response = new PagedList<GadgetReviewResponse>(
                gadgetReviewResponses.Skip(skip).Take(pageSize).ToList(),
                page,
                pageSize,
                gadgetReviewResponses.Count
            );
            return Ok(response);
        }
        else
        {
            var query = context.SellerOrders
                 .Include(so => so.SellerOrderItems)
                    .ThenInclude(soi => soi.Gadget.Category)
                .Include(so => so.SellerOrderItems)
                    .ThenInclude(soi => soi.Review)
                        .ThenInclude(r => r != null ? r.SellerReply : null)
                        .ThenInclude(sr => sr != null ? sr.Seller : null)
                .Include(so => so.SellerOrderItems)
                    .ThenInclude(soi => soi.Review)
                        .ThenInclude(r => r != null ? r.Customer : null)
                .Where(so => so.SellerId == currentUser.Seller!.Id)
                .AsQueryable();

            if (request.FilterBy != FilterBy.Replied && request.FilterBy != FilterBy.NotReply)
            {
                throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEA_00)
                .AddReason("filterBy", "Seller dùng sai filterBy.")
                .Build();
            }

            // Sort theo SortByDate
            if (request.SortByDate != null)
            {
                query = request.SortByDate == SortByDate.ASC
                    ? query.OrderBy(od => od.CreatedAt)
                    : query.OrderByDescending(od => od.CreatedAt);
            }

            var sellerOrders = await query.ToListAsync();

            List<GadgetReviewResponse> gadgetReviewResponses = new List<GadgetReviewResponse>()!;
            foreach (var so in sellerOrders)
            {
                var sellerOrderItems = so.SellerOrderItems;
                foreach (var soi in sellerOrderItems)
                {
                    bool isReviewed = soi.Review != null;
                    bool isReplied = isReviewed && soi.Review!.SellerReply != null;
                    bool isReviewBanned = isReviewed && soi.Review!.Status == ReviewStatus.Inactive;

                    //Nếu review bị banned thì sẽ không add item này vào
                    if (isReviewBanned)
                    {
                        continue;
                    }

                    if (request.FilterBy == FilterBy.Replied && isReviewed)
                    {
                        if (isReplied)
                        {
                            GadgetReviewResponse gadgetReviewResponse = new GadgetReviewResponse()
                            {
                                SellerOrderItemId = soi.Id,
                                GadgetId = soi.GadgetId,
                                Name = soi.Gadget.Name,
                                ThumbnailUrl = soi.Gadget.ThumbnailUrl,
                                Review = soi.Review!.ToReviewResponse(),
                                Status = soi.Gadget.Status,
                            }!;
                            gadgetReviewResponses.Add(gadgetReviewResponse);
                        }
                    }
                    if (request.FilterBy == FilterBy.NotReply && isReviewed)
                    {
                        if (!isReplied && soi.Review!.CreatedAt > DateTime.UtcNow.AddMinutes(-10))
                        {
                            GadgetReviewResponse gadgetReviewResponse = new GadgetReviewResponse()
                            {
                                SellerOrderItemId = soi.Id,
                                GadgetId = soi.GadgetId,
                                Name = soi.Gadget.Name,
                                ThumbnailUrl = soi.Gadget.ThumbnailUrl,
                                Review = soi.Review!.ToReviewResponse(),
                                Status = soi.Gadget.Status,
                            }!;
                            gadgetReviewResponses.Add(gadgetReviewResponse);
                        }
                    }
                }
            }

            int page = request.Page == null ? 1 : (int)request.Page;
            int pageSize = request.PageSize == null ? 10 : (int)request.PageSize;
            int skip = (page - 1) * pageSize;

            var response = new PagedList<GadgetReviewResponse>(
                gadgetReviewResponses.Skip(skip).Take(pageSize).ToList(),
                page,
                pageSize,
                gadgetReviewResponses.Count
            );

            return Ok(response);
        }
    }
}
