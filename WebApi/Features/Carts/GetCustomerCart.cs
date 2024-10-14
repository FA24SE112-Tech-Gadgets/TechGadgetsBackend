using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;
using Microsoft.EntityFrameworkCore;
using WebApi.Features.Carts.Mappers;
using WebApi.Features.Carts.Models;

namespace WebApi.Features.Carts;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class GetCustomerCart : ControllerBase
{
    [HttpGet("carts")]
    [Tags("Carts")]
    [SwaggerOperation(
        Summary = "Get Customer Cart",
        Description = "API is for get customer cart."
    )]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] PagedRequest request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();
        var userCart = await context.Carts
            .Include(c => c.CartGadgets)
            .FirstOrDefaultAsync(c => c.CustomerId == currentUser!.Customer!.Id);

        return Ok(userCart!.ToCartResponse());
    }
}
