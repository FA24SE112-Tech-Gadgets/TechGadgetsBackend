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
[RequestValidation<Request>]
public class UpdateCartItem : ControllerBase
{
    public new class Request
    {
        public Guid GadgetId { get; set; }
        public int Quantity { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(sp => sp.GadgetId)
                .NotEmpty()
                .WithMessage("GadgetId không được để trống");
            RuleFor(sp => sp.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Số lượng không được nhỏ hơn 0");
        }
    }

    [HttpPut("cart/old")]
    [Tags("Carts")]
    [SwaggerOperation(
        Summary = "Update Cart Item",
        Description = "This API is for update customer cart item. Note:" +
                            "<br>&nbsp; - User bị Inactive thì không update cart được."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
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

        var isGadgetExist = await context.Gadgets
            .AnyAsync(g => g.Id == request.GadgetId);
        if (!isGadgetExist)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("gadget", "Không tìm thấy sản phẩm này.")
            .Build();
        }

        if (await context.Gadgets.AnyAsync(g => g.Id == request.GadgetId && g.Status == GadgetStatus.Inactive))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("gadget", "Sản phẩm đã bị khoá.")
                .Build();
        }

        if (await context.Gadgets.AnyAsync(g => g.Id == request.GadgetId && g.IsForSale == false))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("gadget", "Sản phẩm đang trong trạng thái ngừng kinh doanh.")
                .Build();
        }

        // Tìm kiếm CartGadget dựa trên CartId và GadgetId
        var existingCartGadget = await context.CartGadgets
            .FirstOrDefaultAsync(cg => cg.CartId == userCart!.Id && cg.GadgetId == request.GadgetId);

        // Nếu không tìm thấy Gadget và số lượng > 0, thêm Gadget mới vào Cart
        if (existingCartGadget == null && request.Quantity > 0)
        {
            var newCartGadget = new CartGadget
            {
                CartId = userCart!.Id,
                GadgetId = request.GadgetId,
                Quantity = request.Quantity
            };
            await context.CartGadgets.AddAsync(newCartGadget);
        }
        else if (existingCartGadget != null) // Tìm thấy Gadget
        {
            if (request.Quantity == 0)
            {
                context.CartGadgets.Remove(existingCartGadget);
            }
            else
            {
                existingCartGadget.Quantity = request.Quantity;
                context.CartGadgets.Update(existingCartGadget);
            }
        }
        else
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("carts", "Vui lòng nhập đúng số lượng.")
            .Build();
        }

        // Lưu thay đổi vào database
        await context.SaveChangesAsync();

        return Ok("Cập nhật thành công");
    }
}
