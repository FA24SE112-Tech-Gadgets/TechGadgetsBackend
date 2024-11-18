using FluentValidation;
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
public class UpdateGadgetQuantity : ControllerBase
{
    public new class Request
    {
        public int Quantity { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Số lượng phải lớn hơn hoặc bằng 0")
                .LessThanOrEqualTo(1000).WithMessage("Số lượng phải nhỏ hơn 1000");
        }
    }

    [HttpPut("gadgets/{id}/quantity")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Update Gadget Quantity",
        Description = "API for Seller to update gadget quantity"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, Request request, AppDbContext context, CurrentUserService currentUserService)
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

        gadget.Quantity = request.Quantity;
        gadget.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return Ok("Cập nhật thành công");
    }
}
