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
public class CustomerCreateDeleteFavoriteGadget : ControllerBase
{

    [HttpPost("favorite-gadgets/{gadgetId}")]
    [Tags("Favorite Gadgets")]
    [SwaggerOperation(
        Summary = "Customer Create/Delete Favorite Gadget",
        Description = "This API is for customer create/delete favorite gadget"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid gadgetId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var isGadgetExist = await context.Gadgets
            .AnyAsync(g => g.Id == gadgetId);
        if (!isGadgetExist)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerApplication", "Không tìm thấy sản phẩm này.")
            .Build();
        }

        var favoriteGadget = await context.FavoriteGadgets
            .FirstOrDefaultAsync(fg => fg.GadgetId == gadgetId && fg.CustomerId == currentUser!.Customer!.Id);

        if (favoriteGadget == null)
        {
            FavoriteGadget fg = new FavoriteGadget
            {
                CustomerId = currentUser!.Customer!.Id,
                GadgetId = gadgetId,
                CreatedAt = DateTime.UtcNow,
            }!;
            await context.FavoriteGadgets.AddAsync(fg);
        } else
        {
            context.FavoriteGadgets.Remove(favoriteGadget);
        }
        await context.SaveChangesAsync();

        return Ok();
    }
}
