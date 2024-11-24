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

namespace WebApi.Features.Reviews;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
[RequestValidation<Request>]
public class GetReviewsByGadgetIdByManager : ControllerBase
{
    public new class Request : PagedRequest
    {
        public SortByDate? SortByDate { get; set; }
        public bool? IsPositive { get; set; }
        public SortByRating? SortByRating { get; set; }
    }

    public enum SortByDate
    {
        DESC, ASC
    }

    public enum SortByRating
    {
        DESC, ASC
    }

    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("reviews/gadget/{gadgetId}/manager")]
    [Tags("Reviews")]
    [SwaggerOperation(
        Summary = "Get Reviews By GadgetId By Manager",
        Description = "API is for get list of reviews by gadgetId by manager. Note:" +
                            "<br>&nbsp; - Có thể filter theo mới nhất/cũ nhất." +
                            "<br>&nbsp; - Có thể filter theo đánh giá tích cực/tiêu cực." +
                            "<br>&nbsp; - Có thể filter theo đánh giá cao/thấp." +
                            "<br>&nbsp; - Nếu không truyền thì không filter theo kiểu đó." +
                            "<br>&nbsp; - Truyền bao nhiêu filter bấy nhiêu."
    )]
    [ProducesResponseType(typeof(PagedList<ReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, [FromRoute] Guid gadgetId, AppDbContext context)
    {
        if (!await context.Gadgets.AnyAsync(g => g.Id == gadgetId))
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("gadget", "Sản phẩm không tồn tại")
                        .Build();
        }

        var query = context.Gadgets
            .Include(g => g.SellerOrderItems)
            .Where(g => g.Id == gadgetId)
            .SelectMany(g => g.SellerOrderItems)
            .Include(soi => soi.Review)
                .ThenInclude(r => r != null ? r.SellerReply : null)
                .ThenInclude(sr => sr != null ? sr.Seller : null)
            .Include(soi => soi.Review)
                .ThenInclude(r => r != null ? r.Customer : null)
            .Include(soi => soi.Gadget.Category)
            .AsQueryable();

        query = query.Where(soi => soi.Review != null);

        if (request.IsPositive != null)
        {
            query = query.Where(soi => soi.Review!.IsPositive == request.IsPositive);
        }

        if (request.SortByRating != null)
        {
            query = request.SortByRating == SortByRating.ASC
            ? query.OrderBy(soi => soi.Review!.Rating)
            : query.OrderByDescending(soi => soi.Review!.Rating);
        }

        if (request.SortByDate != null)
        {
            query = request.SortByDate == SortByDate.ASC
                ? query.OrderBy(soi => soi.Review!.CreatedAt)
                : query.OrderByDescending(soi => soi.Review!.CreatedAt);
        }

        var reviews = await query.ToPagedListAsync(request);

        var reviewsResponse = new PagedList<ReviewResponse>(
            reviews.Items.Select(soi => soi.Review!.ToReviewResponse()!).ToList(),
            reviews.Page,
            reviews.PageSize,
            reviews.TotalItems
        );

        return Ok(reviewsResponse);
    }
}
