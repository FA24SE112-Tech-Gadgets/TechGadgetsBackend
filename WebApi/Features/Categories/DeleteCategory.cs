using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Categories;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class DeleteCategory : ControllerBase
{
    [HttpDelete("categories/{id}")]
    [Tags("Categories")]
    [SwaggerOperation(
        Summary = "Manager Delete Categoy"
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

        if (!await context.Categories.AnyAsync(b => b.Id == id))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("category", "Không tìm thấy thể loại.")
                .Build();
        }

        if (await context.Gadgets.AnyAsync(g => g.CategoryId == id))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("brand", "Thể loại đang được tham chiếu tới, không thể xoá.")
                .Build();
        }

        await context.Categories
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync();

        return Ok("Xóa thể loại thành công");
    }
}

