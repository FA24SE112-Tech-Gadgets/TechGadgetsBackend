using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Gadgets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Seller)]
public class DeleteGadget : ControllerBase
{
    [HttpDelete("gadgets/{id}")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Seller Delete Gadget"
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

        var gadget = await context.Gadgets.FirstOrDefaultAsync(g => g.Id == id);
        if (gadget is null)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("gadget", "Sản phẩm không tồn tại")
                        .Build();
        }

        if (gadget.SellerId != currentUser!.Seller!.Id)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEA_00)
                .AddReason("role", "Tài khoản không đủ thẩm quyền để thực hiện hành động này.")
                .Build();
        }

        if (gadget.Status == GadgetStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEA_00)
                .AddReason("role", "Sản phẩm đã bị khoá, bạn không thể thực hiện hành động này")
                .Build();
        }

        var existsGadgetHistory = await context.GadgetHistories.AnyAsync(gh => gh.GadgetId == id);
        var existsFavoriteGadget = await context.FavoriteGadgets.AnyAsync(gh => gh.GadgetId == id);
        var existsCartGadget = await context.CartGadgets.AnyAsync(gh => gh.GadgetId == id);
        var existsSellerOrderItem = await context.SellerOrderItems.AnyAsync(gh => gh.GadgetId == id);

        if (existsGadgetHistory || existsFavoriteGadget || existsCartGadget || existsSellerOrderItem)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("gadget", "Sản phẩm đang được tham chiếu tới, không thể xoá.")
                .Build();
        }

        await context.Gadgets
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync();

        return Ok("Xóa sản phẩm thành công");
    }
}
