using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Carts;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class DeleteCustomerCartItems : ControllerBase
{
    [HttpDelete("carts/clear")]
    [Tags("Carts")]
    [SwaggerOperation(
        Summary = "Clear Customer Cart Items",
        Description = "This API is for clearing all items in the customer's cart."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();
        var userCart = await context.Carts
            .FirstOrDefaultAsync(c => c.CustomerId == currentUser!.Customer!.Id);

        if (userCart == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("carts", "Không tìm thấy giỏ hàng.")
            .Build();
        }

        var cartGadgetsToDelete = await context.CartGadgets
            .Where(cg => cg.CartId == userCart.Id)
            .ToListAsync();

        if (cartGadgetsToDelete.Count == 0)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("cartGadgets", "Giỏ hàng đã trống.")
            .Build();
        }

        context.CartGadgets.RemoveRange(cartGadgetsToDelete);
        await context.SaveChangesAsync();

        return Ok();
    }
}
