using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.NaturalLanguageKeywords;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class DeleteCriteriaById : ControllerBase
{
    [HttpDelete("natural-language-keyword-groups/criteria/{id}")]
    [Tags("Natural Language Keywords")]
    [SwaggerOperation(
        Summary = "Manager Delete Criteria By Id"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
                .Build();
        }

        var criteria = await context.Criteria
                                    .Include(c => c.NaturalLanguageKeywordGroup.Criteria)
                                    .FirstOrDefaultAsync(g => g.Id == id);

        if (criteria == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("criteria", "Tiêu chí không tồn tại.")
                .Build();
        }

        if (criteria.NaturalLanguageKeywordGroup.Criteria.Count == 1)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("criteria", "Không thể xoá tiêu chí, nhóm từ khoá cần ít nhất 1 tiêu chí.")
                .Build();
        }

        var now = DateTime.UtcNow;

        await context.Criteria.Where(k => k.Id == id).ExecuteDeleteAsync();

        criteria.NaturalLanguageKeywordGroup.UpdatedAt = now;

        await context.SaveChangesAsync();

        return Ok("Xoá thành công");
    }
}
