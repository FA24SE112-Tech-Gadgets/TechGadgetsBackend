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
public class SetGadgetForSale : ControllerBase
{
    [HttpPut("gadgets/{id}/set-for-sale")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Set Gadget To For Sale",
        Description = "API for Seller to set gadget to for sale."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, AppDbContext context, CurrentUserService currentUserService)
    {
        var gadget = await context.Gadgets.FirstOrDefaultAsync(g => g.Id == id);
        if (gadget is null)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("gadget", "Sản phẩm không tồn tại")
                        .Build();
        }

        var currentUser = await currentUserService.GetCurrentUser();
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

        if (gadget.IsForSale == true)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("gadget", "Sản phẩm đã trong trạng thái đang kinh doanh từ trước")
                        .Build();
        }

        gadget.IsForSale = true;

        await context.SaveChangesAsync();

        return Ok();
    }
}
