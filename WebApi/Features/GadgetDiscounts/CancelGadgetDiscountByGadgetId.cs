using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.GadgetDiscounts;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Seller)]
public class CancelGadgetDiscountByGadgetId : ControllerBase
{
    [HttpPut("gadget-discount/{gadgetId}")]
    [Tags("Gadget Discounts")]
    [SwaggerOperation(Summary = "Cancel Gadget Discount By GadgetId",
        Description = """
        This API is for cancel gadget discount by gadgetId
        - Dùng API để huỷ giảm giá hiện tại cho gadget
        """
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context, [FromRoute] Guid gadgetId, CurrentUserService currentUserService)
    {
        var user = await currentUserService.GetCurrentUser();

        if (user!.Seller is null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("seller", "Seller chưa được kích hoạt")
                .Build();
        }

        if (user.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("seller", "Tài khoản Seller đã bị khoá")
                .Build();
        }

        var currGadget = await context.Gadgets
            .FirstOrDefaultAsync(g => g.Id == gadgetId);

        if (currGadget == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("gadget", "Sản phẩm không tồn tại")
                .Build();
        }

        if (currGadget.SellerId != user.Seller.Id)
        {
            throw TechGadgetException.NewBuilder()
                    .WithCode(TechGadgetErrorCode.WEB_02)
                    .AddReason("seller", "Người dùng không đủ thẩm quyền.")
                    .Build();
        }

        if (currGadget.Status == GadgetStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("gadget", "Sản phẩm đã bị khoá.")
                .Build();
        }

        var existGadgetDiscount = await context.GadgetDiscounts
            .FirstOrDefaultAsync(gd => gd.GadgetId == gadgetId && gd.Status == GadgetDiscountStatus.Active);

        if (existGadgetDiscount != null)
        {
            existGadgetDiscount.Status = GadgetDiscountStatus.Cancelled;
        }
        else
        {
            throw TechGadgetException.NewBuilder()
                    .WithCode(TechGadgetErrorCode.WEB_02)
                    .AddReason("gadgetDiscount", "Sản phẩm hiện đang không được giảm giá.")
                    .Build();
        }

        await context.SaveChangesAsync();

        return Ok("Huỷ giảm giá thành công");
    }
}
