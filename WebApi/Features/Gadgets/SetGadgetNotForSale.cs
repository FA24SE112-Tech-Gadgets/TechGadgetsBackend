﻿using Microsoft.AspNetCore.Mvc;
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
public class SetGadgetNotForSale : ControllerBase
{
    [HttpPut("gadgets/{id}/set-not-for-sale")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Set Gadget To Not For Sale",
        Description = "API for Seller to set gadget to not for sale. Note:" +
                            "<br>&nbsp; - User bị Inactive thì không cập nhật not for sale được."
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
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("role", "Sản phẩm đã bị khoá, bạn không thể thực hiện hành động này")
                .Build();
        }

        if (gadget.IsForSale == false)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("gadget", "Sản phẩm đã ngừng kinh doanh từ trước")
                        .Build();
        }

        gadget.IsForSale = false;
        gadget.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        await context.GadgetDiscounts
                       .Where(gd => gd.GadgetId == id && gd.Status == GadgetDiscountStatus.Active)
                       .ExecuteUpdateAsync(setters => setters.SetProperty(gd => gd.Status, GadgetDiscountStatus.Cancelled));

        return Ok("Cập nhật thành công");
    }
}
