using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Data;
using WebApi.Features.Reviews.Models;

namespace WebApi.Features.Reviews;

[ApiController]
public class GetReviewSummaryByGadgetId : ControllerBase
{
    [HttpGet("reviews/summary/gadgets/{gadgetId}")]
    [Tags("Reviews")]
    [SwaggerOperation(
        Summary = "Get Review Summary By GadgetId"
    )]
    [ProducesResponseType(typeof(ReviewSummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid gadgetId, AppDbContext context)
    {
        if (!await context.Gadgets.AnyAsync(g => g.Id == gadgetId))
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("gadget", "Sản phẩm không tồn tại")
                        .Build();
        }

        var reviewSummary = await context.Reviews
                                        .Where(r => r.SellerOrderItem.GadgetId == gadgetId && r.Rating >= 1 && r.Rating <= 5)
                                        .GroupBy(r => r.Rating)
                                        .Select(g => new
                                        {
                                            Rating = g.Key,
                                            Count = g.Count()
                                        })
                                        .ToListAsync();

        var totalReviews = reviewSummary.Sum(r => r.Count);
        double avgReview = totalReviews > 0
                                ? (double)reviewSummary.Sum(r => r.Rating * r.Count) / totalReviews
                                : 0;

        avgReview = Math.Round(avgReview, 1);

        var reviewSummaryResponse = new ReviewSummaryResponse
        {
            AvgReview = avgReview,
            NumOfReview = totalReviews,
            NumOfFiveStar = reviewSummary.FirstOrDefault(r => r.Rating == 5)?.Count ?? 0,
            NumOfFourStar = reviewSummary.FirstOrDefault(r => r.Rating == 4)?.Count ?? 0,
            NumOfThreeStar = reviewSummary.FirstOrDefault(r => r.Rating == 3)?.Count ?? 0,
            NumOfTwoStar = reviewSummary.FirstOrDefault(r => r.Rating == 2)?.Count ?? 0,
            NumOfOneStar = reviewSummary.FirstOrDefault(r => r.Rating == 1)?.Count ?? 0
        };

        return Ok(reviewSummaryResponse);
    }
}
