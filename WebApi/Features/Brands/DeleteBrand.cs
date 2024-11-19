using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Brands;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class DeleteBrand : ControllerBase
{
    [HttpDelete("brands/{id}")]
    [Tags("Brands")]
    [SwaggerOperation(
        Summary = "Manager Delete Brand"
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

        if (!await context.Brands.AnyAsync(b => b.Id == id))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("brand", "Không tìm thấy thương hiệu.")
                .Build();
        }

        if (await context.Gadgets.AnyAsync(g => g.BrandId == id))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("brand", "Thương hiệu đang được tham chiếu tới, không thể xoá.")
                .Build();
        }

        await context.Brands
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync();

        return Ok("Xóa thương hiệu thành công");
    }
}
