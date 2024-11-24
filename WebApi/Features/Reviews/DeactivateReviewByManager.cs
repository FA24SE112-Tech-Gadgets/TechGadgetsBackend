using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;

namespace WebApi.Features.Reviews;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class DeactivateReviewByManager : ControllerBase
{
    [HttpPut("review/{id}/deactivate")]
    [Tags("Reviews")]
    [SwaggerOperation(
        Summary = "Manager Deactivate Review"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, AppDbContext context)
    {
        var review = await context.Reviews.FirstOrDefaultAsync(g => g.Id == id);
        if (review is null)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("review", "Đánh giá không tồn tại.")
                        .Build();
        }

        if (review.Status == ReviewStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("review", "Đánh giá đã trong trạng thái inactive.")
                        .Build();
        }

        review.Status = ReviewStatus.Inactive;

        await context.SaveChangesAsync();

        return Ok("Cập nhật thành công");
    }
}
