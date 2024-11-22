using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Stats;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Seller)]
public class GetNumberOfSoldGadgetsBySeller : ControllerBase
{
    [HttpGet("stats/number-of-sold-gadgets/seller")]
    [Tags("Stats")]
    [SwaggerOperation(
        Summary = "Seller Gets Number Of Sold Gadgets",
        Description = "API is for seller gets number of sold gadgets"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var response = await context.SellerOrderItems
                                    .Where(soi => soi.SellerOrder.SellerId == currentUser!.Seller!.Id)
                                    .Where(soi => soi.SellerOrder.Status == SellerOrderStatus.Success)
                                    .SumAsync(s => s.GadgetQuantity);

        return Ok(new
        {
            numberOfSoldGadgets = response
        });
    }
}
