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
public class GetNumberOfGadgetsBySeller : ControllerBase
{
    [HttpGet("stats/number-of-gadgets/seller")]
    [Tags("Stats")]
    [SwaggerOperation(
        Summary = "Seller Gets Number Of Gadgets",
        Description = "API is for seller gets number of gadgets"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var response = await context.Gadgets.Where(g => g.SellerId == currentUser!.Seller!.Id).CountAsync();

        return Ok(new
        {
            numberOfGadgets = response
        });
    }
}
