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
public class GetReviewsHomePage : ControllerBase
{
    public new class Request : PagedRequest;

    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("reviews/home-page")]
    [Tags("Reviews")]
    [SwaggerOperation(
        Summary = "Get Reviews Home Page",
        Description = "API is for get list of reviews in home page"
    )]
    [ProducesResponseType(typeof(PagedList<ReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context)
    {
        var query = context.Gadgets
            .Include(g => g.SellerOrderItems)
            .SelectMany(g => g.SellerOrderItems)
            .Include(soi => soi.Review)
                .ThenInclude(r => r != null ? r.SellerReply : null)
                .ThenInclude(sr => sr != null ? sr.Seller : null)
            .Include(soi => soi.Review)
                .ThenInclude(r => r != null ? r.Customer : null)
            .Include(soi => soi.Gadget.Category)
            .AsQueryable();

        query = query.Where(soi => soi.Review != null && soi.Review.Status == Data.Entities.ReviewStatus.Active)
                     .Where(soi => soi.Review.IsPositive == true)
                     .OrderByDescending(soi => soi.Review!.Rating);

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
