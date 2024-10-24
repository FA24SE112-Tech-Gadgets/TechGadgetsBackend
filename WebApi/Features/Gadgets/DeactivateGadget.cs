﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;

namespace WebApi.Features.Gadgets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class DeactivateGadget : ControllerBase
{
    [HttpPut("gadgets/{id}/deactivate")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Deactivate Gadget",
        Description = "API for Manager to deactivate gadget."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, AppDbContext context)
    {
        var gadget = await context.Gadgets.FirstOrDefaultAsync(g => g.Id == id);
        if (gadget is null)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("gadget", "Sản phẩm không tồn tại")
                        .Build();
        }

        if (gadget.Status == GadgetStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("gadget", "Sản phẩm đã bị vô hiệu hoá từ trước")
                        .Build();
        }

        gadget.Status = GadgetStatus.Inactive;
        gadget.IsForSale = false;

        await context.SaveChangesAsync();

        return Ok();
    }
}
