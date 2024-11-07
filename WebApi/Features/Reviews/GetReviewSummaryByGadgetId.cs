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
        var avg = await context.Reviews
                                .Where(r => r.SellerOrderItem.GadgetId == gadgetId)
                                .Select(r => r.Rating)
                                .AverageAsync();

        return Ok(avg);
    }
}
