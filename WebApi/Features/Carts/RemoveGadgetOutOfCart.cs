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
public class RemoveGadgetOutOfCart : ControllerBase
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
                .GreaterThan(0)
                .WithMessage("Số lượng không được nhỏ hơn 1");
        }
    }

    [HttpDelete("cart")]
    [Tags("Carts")]
    [SwaggerOperation(
        Summary = "Remove Gadget Out Of Cart",
        Description = "This API is for customer remove gadget out of cart. Note:" +
                            "<br>&nbsp; - Quantity phải lớn hơn 1." +
                            "<br>&nbsp; - Truyền bao nhiêu remove bấy nhiêu."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
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

        var isGadgetExist = await context.Gadgets
            .AnyAsync(g => g.Id == request.GadgetId);
        if (!isGadgetExist)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("gadget", "Không tìm thấy sản phẩm này.")
            .Build();
        }

        // Tìm kiếm CartGadget dựa trên CartId và GadgetId
        var existingCartGadget = await context.CartGadgets
            .FirstOrDefaultAsync(cg => cg.CartId == userCart!.Id && cg.GadgetId == request.GadgetId);

        // Nếu không tìm thấy Gadget và số lượng > 0, thêm Gadget mới vào Cart
        if (existingCartGadget == null && request.Quantity > 0)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("gadget", $"Sản phẩm {request.GadgetId} không có trong giỏ hàng.")
            .Build();
        }
        else if (existingCartGadget != null) // Tìm thấy Gadget
        {
            int newQuantity = existingCartGadget.Quantity - request.Quantity;
            if (newQuantity == 0)
            {
                context.CartGadgets.Remove(existingCartGadget);
            }
            else if (newQuantity > 0)
            {
                existingCartGadget.Quantity = newQuantity;
                context.CartGadgets.Update(existingCartGadget);
            } else
            {
                throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("carts", "Số lượng muốn xóa vượt quá số sản phẩm có trong giỏ hàng.")
                .Build();
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

        return Ok();
    }
}
