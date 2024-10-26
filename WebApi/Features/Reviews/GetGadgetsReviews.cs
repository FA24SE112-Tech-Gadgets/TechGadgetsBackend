using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Reviews.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Services.Auth;
using WebApi.Features.Reviews.Mappers;

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

    [HttpGet("reviews/gadgets")]
    [Tags("Reviews")]
    [SwaggerOperation(
        Summary = "Get List Gadgets Need Review/Reviewed/Need Reply/Replied",
        Description = "API is for get list of gadgets need review/reviewed/need reply/replied base on Customer/Seller. Note:" +
                            "<br>&nbsp; - FilterBy: 'NotReview', 'Reviewed', 'NotReply', 'Replied'. Default: 'NotReview'" +
                            "<br>&nbsp; - Dùng API này để lấy ra danh sách các sản phầm chưa đánh giá/đã đánh giá (Customer) và chưa phản hồi/đã phản hồi (Seller)." +
                            "<br>&nbsp; - Customer: NotReview, Reviewed." +
                            "<br>&nbsp; - Seller: NotReply, Replied."
    )]
    [ProducesResponseType(typeof(PagedList<GadgetReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        //var currentUser = await currentUserService.GetCurrentUser();

        //if (currentUser!.Role == Role.Customer)
        //{
        //    var query = context.Orders
        //    .Include(o => o.SellerOrders)
        //        .ThenInclude(od => od.Reviews)
        //    .Where(o => o.CustomerId == currentUser!.Customer!.Id)
        //    .SelectMany(o => o.OrderDetails)
        //    .Include(od => od.GadgetInformation)
        //        .ThenInclude(gi => gi.Gadget)
        //    .AsQueryable();

        //    if (request.FilterBy != FilterBy.Reviewed && request.FilterBy != FilterBy.NotReview)
        //    {
        //        throw TechGadgetException.NewBuilder()
        //        .WithCode(TechGadgetErrorCode.WEA_00)
        //        .AddReason("filterBy", "Customer dùng sai filterBy.")
        //        .Build();
        //    }

        //    // Sort theo SortByDate
        //    if (request.SortByDate != null)
        //    {
        //        query = request.SortByDate == SortByDate.ASC
        //            ? query.OrderBy(od => od.CreatedAt)
        //            : query.OrderByDescending(od => od.CreatedAt);
        //    }

        //    var orderDetails = await query.ToListAsync();

        //    List<GadgetReviewResponse> gadgetReviewResponses = new List<GadgetReviewResponse>()!;
        //    foreach (var od in orderDetails)
        //    {
        //        var gadgetinformations = od.GadgetInformation;
        //        foreach (var gi in gadgetinformations)
        //        {
        //            if (request.FilterBy == FilterBy.Reviewed)
        //            {
        //                var gadgetReviews = await context.Reviews
        //                    .Include(r => r.SellerReply)
        //                    .Include(r => r.Gadget)
        //                    .Where(r => r.GadgetId == gi.GadgetId && r.OrderDetailId == od.Id)
        //                    .Select(r => r.ToGadgetReviewResponse()!)
        //                    .ToListAsync();
        //                gadgetReviewResponses.AddRange(gadgetReviews);
        //            }
        //            else
        //            {
        //                bool isReviewed = await context.Reviews.AnyAsync(r => r.GadgetId == gi.GadgetId && r.OrderDetailId == od.Id);
        //                if (!isReviewed)
        //                {
        //                    GadgetReviewResponse gadgetReviewResponse = new GadgetReviewResponse()
        //                    {
        //                        Id = gi.GadgetId,
        //                        Name = gi.Gadget.Name,
        //                        ThumbnailUrl = gi.Gadget.ThumbnailUrl,
        //                        Status = gi.Gadget.Status,
        //                    }!;
        //                    gadgetReviewResponses.Add(gadgetReviewResponse);
        //                }
        //            }
        //        }
        //    }
        //    int page = request.Page == null ? 0 : (int)request.Page;
        //    int pageSize = request.PageSize == null ? 10 : (int)request.PageSize;
        //    int skip = (page - 1) * pageSize;

        //    var response = new PagedList<GadgetReviewResponse>(
        //        gadgetReviewResponses.Skip(skip).Take(pageSize).ToList(),
        //        page,
        //        pageSize,
        //        gadgetReviewResponses.Count
        //    );
        //    return Ok(response);
        //}
        //else
        //{

        //    return Ok();
        //}
        return Ok();
    }
}
