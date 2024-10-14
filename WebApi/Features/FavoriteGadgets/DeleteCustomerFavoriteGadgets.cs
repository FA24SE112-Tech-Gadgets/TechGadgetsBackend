using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.FavoriteGadgets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class DeleteCustomerFavoriteGadgets : ControllerBase
{
    [HttpDelete("favorite-gadgets")]
    [Tags("Favorite Gadgets")]
    [SwaggerOperation(
        Summary = "Customer Delete All Favorite Gadgets",
        Description = "This API is for customer delete all favorite gadgets"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var favoriteGadgetsToDelete = await context.FavoriteGadgets
            .Where(fg => fg.CustomerId == currentUser!.Customer!.Id)
            .ToListAsync();

        // Remove the selected gadgets from the context
        context.FavoriteGadgets.RemoveRange(favoriteGadgetsToDelete);

        // Save changes to persist the deletion
        await context.SaveChangesAsync();

        return Ok();
    }
}
