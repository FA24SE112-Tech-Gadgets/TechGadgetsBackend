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
public class DeleteAllKeywordHistory : ControllerBase
{
    [HttpDelete("keyword-histories/all")]
    [Tags("Keyword Histories")]
    [SwaggerOperation(
        Summary = "User Delete All Keyword Histories"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        await context.KeywordHistories
                    .Where(b => b.UserId == currentUser!.Id)
                    .ExecuteDeleteAsync();

        return Ok("Xóa tất cả từ khoá tìm kiếm thành công");
    }
}
