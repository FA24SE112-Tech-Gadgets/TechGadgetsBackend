using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Services.Auth;

namespace WebApi.Features.KeywordHistories;

[ApiController]
[JwtValidation]
public class DeleteKeywordHistoryById : ControllerBase
{
    [HttpDelete("keyword-histories/{id}")]
    [Tags("Keyword Histories")]
    [SwaggerOperation(
        Summary = "User Delete Keyword History By Id"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var keywordHistory = await context.KeywordHistories.FirstOrDefaultAsync(b => b.Id == id);

        if (keywordHistory is null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("keywordHistory", "Không tìm thấy từ khoá tìm kiếm.")
                .Build();
        }

        if (keywordHistory.UserId != currentUser!.Id)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEA_00)
                .AddReason("role", "Tài khoản không đủ thẩm quyền để thực hiện hành động này.")
                .Build();
        }

        await context.KeywordHistories
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync();

        return Ok("Xóa từ khoá tìm kiếm thành công");
    }
}
