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
using WebApi.Features.GadgetHistories.Models;
using WebApi.Features.Users.Mappers;
using WebApi.Services.Auth;

namespace WebApi.Features.Carts;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class GetListSellerInCart : ControllerBase
{
    [HttpGet("carts/sellers")]
    [Tags("Carts")]
    [SwaggerOperation(
        Summary = "Get List Sellers In Customer Cart",
        Description = "This API is for get list seller in customer cart. Note:" +
                            "<br>&nbsp; - Dùng API này để lấy thông Seller và SellerId và kết hợp với API GetCartItemsBySellerId để lấy được Gadget theo từng Seller." +
                            "<br>&nbsp; - Dùng để show thông tin của Seller như ShopName." +
                            "<br>&nbsp; - Trạng thái tài khoản của Seller (user.status)." +
                            "<br>&nbsp; - Nếu Seller bị khóa thì không được thanh toán đơn đó (Đơn của Seller đó thôi)." +
                            "<br>&nbsp; - Có thể show số điện thoại vs companyName (Tùy FE style ntn)."
    )]
    [ProducesResponseType(typeof(PagedList<SellerCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] PagedRequest request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        // Truy vấn để lấy seller từ giỏ hàng của user
        var sellersQuery = await context.Carts
            .Where(c => c.CustomerId == currentUser!.Customer!.Id)   // Lọc theo giỏ hàng của user
            .SelectMany(c => c.CartGadgets.Select(cg => cg.Gadget.Seller)) // Lấy seller từ các sản phẩm trong giỏ hàng
            .Distinct()  // Loại bỏ seller trùng lặp
            .Include(s => s.User)
            .OrderBy(s => s.Id) // Sắp xếp để có thể phân trang
            .ToPagedListAsync(request);

        if (sellersQuery == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("carts", "Không tìm thấy giỏ hàng.")
            .Build();
        } else if (sellersQuery.TotalItems == 0)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("carts", "Giỏ hàng rỗng.")
            .Build();
        }

        var sellersResponseList = new PagedList<SellerCartResponse>(
            sellersQuery.Items.Select(s => s.ToSellerCartResponse()!).ToList(),
            sellersQuery.Page,
            sellersQuery.PageSize,
            sellersQuery.TotalItems
        );

        return Ok(sellersResponseList);
    }
}
