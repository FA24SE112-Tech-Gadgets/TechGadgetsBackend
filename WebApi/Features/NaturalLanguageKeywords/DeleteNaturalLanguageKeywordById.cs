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
public class DeleteNaturalLanguageKeywordById : ControllerBase
{
    [HttpDelete("natural-language-keyword-groups/natural-language-keywords/{id}")]
    [Tags("Natural Language Keywords")]
    [SwaggerOperation(
        Summary = "Manager Delete Natural Language Keyword By Id"
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

        var keyword = await context.NaturalLanguageKeywords
                                    .Include(k => k.NaturalLanguageKeywordGroup.NaturalLanguageKeywords)
                                    .FirstOrDefaultAsync(g => g.Id == id);

        if (keyword == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("naturalLanguageKeyword", "Từ khoá không tồn tại.")
                .Build();
        }

        if (keyword.NaturalLanguageKeywordGroup.NaturalLanguageKeywords.Count == 1)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("naturalLanguageKeyword", "Không thể xoá từ khoá, nhóm từ khoá cần ít nhất 1 từ khoá.")
                .Build();
        }

        var now = DateTime.UtcNow;

        await context.NaturalLanguageKeywords.Where(k => k.Id == id).ExecuteDeleteAsync();

        keyword.NaturalLanguageKeywordGroup.UpdatedAt = now;

        await context.SaveChangesAsync();

        return Ok("Xoá thành công");
    }
}
