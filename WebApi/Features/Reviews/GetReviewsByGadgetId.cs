using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Features.Reviews.Mappers;
using WebApi.Features.Reviews.Models;

namespace WebApi.Features.Reviews;

[ApiController]
public class GetReviewsByGadgetId : ControllerBase
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

    [HttpGet("reviews/gadget/{gadgetId}")]
    [Tags("Reviews")]
    [SwaggerOperation(
        Summary = "Get Reviews By GadgetId",
        Description = "API is for get list of reviews by gadgetId. Note:" +
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
        var query = context.Reviews
            .Include(r => r.SellerReply)
            .AsQueryable();

        query.Where(r => r.GadgetId == gadgetId);

        if (request.IsPositive != null)
        {
            query = query.Where(r => r.IsPositive == request.IsPositive);
        }

        if (request.SortByRating != null)
        {
            query = request.SortByRating == SortByRating.ASC
            ? query.OrderBy(r => r.Rating)
            : query.OrderByDescending(r => r.Rating);
        }

        if (request.SortByDate != null)
        {
            query = request.SortByDate == SortByDate.ASC
                ? query.OrderBy(r => r.CreatedAt)
                : query.OrderByDescending(r => r.CreatedAt);
        }

        var reviews = await query.ToPagedListAsync(request);

        var reviewsResponse = new PagedList<ReviewResponse>(
            reviews.Items.Select(r => r.ToReviewResponse()!).ToList(),
            reviews.Page,
            reviews.PageSize,
            reviews.TotalItems
        );

        return Ok(reviewsResponse);
    }
}
