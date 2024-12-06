using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.NaturalLanguagePrompts;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class DeleteNaturalLanguagePromptById : ControllerBase
{
    [HttpDelete("natural-language-prompts/{id}")]
    [Tags("Natural Language Prompts")]
    [SwaggerOperation(
        Summary = "Manager Delete Natural Language Prompt By Id"
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

        var prompt = await context.NaturalLanguagePrompts
                                    .FirstOrDefaultAsync(g => g.Id == id);

        if (prompt == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("naturalLanguagePrompt", "Prompt không tồn tại.")
                .Build();
        }


        await context.NaturalLanguagePrompts.Where(k => k.Id == id).ExecuteDeleteAsync();

        await context.SaveChangesAsync();

        return Ok("Xoá thành công");
    }
}
