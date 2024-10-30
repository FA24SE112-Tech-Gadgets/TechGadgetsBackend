using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Carts.Mappers;
using WebApi.Features.Carts.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Carts;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class GetCartItemsBySellerId : ControllerBase
{
    [HttpGet("cart/seller/{sellerId}")]
    [Tags("Carts")]
    [SwaggerOperation(
        Summary = "Get Customer Cart Items By SellerId",
        Description = "This API is for get customer cart items by sellerId. Note:" +
                            "<br>&nbsp; - Status: Active | Inactive. Cho Manaager khóa sản phẩm nếu bán hàng cấm này kia." +
                            "<br>&nbsp; - IsForSale: True | Galse. False => Sản phẩm ngừng doanh. Dùng để hiện watermark 'Sản phẩm ngừng kinh doanh' nếu false." +
                            "<br>&nbsp; - Quantity: Nếu quantity = 0 thì hiện sản phẩm hết hàng." +
                            "<br>&nbsp; - Seller không được cập nhật Status của Gadget." +
                            "<br>&nbsp; - Nếu sản phẩm Status bị Inactive thì Seller không được Update IsForSale."
    )]
    [ProducesResponseType(typeof(PagedList<CartGadgetItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] PagedRequest request, [FromRoute] Guid sellerId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();
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

        var cartGadgetBySellerId = await context.CartGadgets
            .Include(cg => cg.Gadget)
                .ThenInclude(g => g.Brand)
            .Include(cg => cg.Gadget)
                .ThenInclude(g => g.Category)
            .Include(cg => cg.Gadget)
                .ThenInclude(g => g.GadgetDiscounts)
            .Where(cg => cg.CartId == userCart.Id && cg.Gadget.SellerId == sellerId)
            .ToPagedListAsync(request);

        if (cartGadgetBySellerId.TotalItems == 0)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("cartGadgets", "Không có sản phẩm nào của seller này trong giỏ hàng.")
            .Build();
        }

        var cartGadgetsResponseList = new PagedList<CartGadgetItemResponse>(
            cartGadgetBySellerId.Items.Select(cg => cg.ToCartGadgetItemResponse()!).ToList(),
            cartGadgetBySellerId.Page,
            cartGadgetBySellerId.PageSize,
            cartGadgetBySellerId.TotalItems
        );

        return Ok(cartGadgetsResponseList);
    }
}
