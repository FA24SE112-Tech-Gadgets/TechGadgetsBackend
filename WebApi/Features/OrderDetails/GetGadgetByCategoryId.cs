using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Services.Auth;
using Microsoft.EntityFrameworkCore;
using WebApi.Features.Gadgets.Mappers;
using WebApi.Features.Gadgets.Models;
using WebApi.Data.Entities;

namespace WebApi.Features.OrderDetails;

[ApiController]
public class GetGadgetByCategoryId : ControllerBase
{
    [HttpGet("gadgets/category/{categoryId}")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Get List Gadgets By CategoryId",
        Description = "API is for get list of gadgets by categoryId."
    )]
    [ProducesResponseType(typeof(PagedList<GadgetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] PagedRequest request, [FromRoute] Guid categoryId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var gadgets = await context.Gadgets
            .Include(c => c.Seller)
                .ThenInclude(s => s.User)
            .Include(c => c.FavoriteGadgets)
            .Where(g => g.CategoryId == categoryId && g.Status == GadgetStatus.Active)
            .Select(c => c.ToGadgetResponse(currentUser!.Customer!.Id))
            .ToPagedListAsync(request);

        return Ok(gadgets);
    }
}
