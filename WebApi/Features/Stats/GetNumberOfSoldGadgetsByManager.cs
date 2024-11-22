using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;

namespace WebApi.Features.Stats;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class GetNumberOfSoldGadgetsByManager : ControllerBase
{
    [HttpGet("stats/number-of-sold-gadgets/manager")]
    [Tags("Stats")]
    [SwaggerOperation(
        Summary = "Manager Gets Number Of Sold Gadgets",
        Description = "API is for manager gets number of sold gadgets"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context)
    {
        var response = await context.SellerOrderItems
                                    .Where(soi => soi.SellerOrder.Status == SellerOrderStatus.Success)
                                    .SumAsync(s => s.GadgetQuantity);

        return Ok(new
        {
            numberOfSoldGadgets = response
        });
    }
}

