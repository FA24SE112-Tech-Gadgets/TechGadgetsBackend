using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Carts;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class DeleteCartItemBySellerId : ControllerBase
{
    [HttpDelete("cart/seller/{sellerId}")]
    [Tags("Carts")]
    [SwaggerOperation(
        Summary = "Delete Customer Cart Items By SellerId",
        Description = "This API is for delete customer cart items by sellerId." +
                            "<br>&nbsp; - User bị Inactive thì không delete customer cart được."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid sellerId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        var userCart = await context.Carts
            .FirstOrDefaultAsync(c => c.CustomerId == currentUser!.Customer!.Id);

        if (userCart == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("carts", "Không tìm thấy giỏ hàng.")
            .Build();
        }

        var isSellerExist = await context.Sellers
            .AnyAsync(s => s.Id == sellerId);
        if (!isSellerExist)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("seller", "Không tìm thấy người bán này.")
            .Build();
        }

        var cartGadgetsToDelete = await context.CartGadgets
            .Include(cg => cg.Gadget)
            .Where(cg => cg.CartId == userCart.Id && cg.Gadget.SellerId == sellerId)
            .ToListAsync();

        if (cartGadgetsToDelete.Count == 0)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("cartGadgets", "Không có sản phẩm nào của seller này trong giỏ hàng.")
            .Build();
        }

        context.CartGadgets.RemoveRange(cartGadgetsToDelete);
        await context.SaveChangesAsync();

        return Ok("Xóa thành công");
    }
}
