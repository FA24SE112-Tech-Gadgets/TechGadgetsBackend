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
        Description = "API for Seller to set gadget to not for sale."
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

        if (gadget.IsForSale == false)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("gadget", "Sản phẩm đã ngừng kinh doanh từ trước")
                        .Build();
        }

        gadget.IsForSale = false;

        await context.SaveChangesAsync();

        return Ok();
    }
}
