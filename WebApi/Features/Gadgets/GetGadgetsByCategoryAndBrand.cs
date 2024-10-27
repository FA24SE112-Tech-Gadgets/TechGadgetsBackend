using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Paginations;
using WebApi.Data.Entities;
using WebApi.Data;
using WebApi.Features.Gadgets.Models;
using WebApi.Services.Auth;
using Microsoft.EntityFrameworkCore;
using WebApi.Features.Gadgets.Mappers;

namespace WebApi.Features.Gadgets;

[ApiController]
public class GetGadgetsByCategoryAndBrand : ControllerBase
{
    [HttpGet("gadgets/category/{categoryId}brand/{brandId}")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Get List Gadgets By CategoryId And BrandId",
        Description = "API is for get list of gadgets by categoryId and brandId."
    )]
    [ProducesResponseType(typeof(PagedList<GadgetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] PagedRequest request, [FromRoute] Guid categoryId, [FromRoute] Guid brandId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var gadgets = await context.Gadgets
            .Include(c => c.Seller)
                .ThenInclude(s => s.User)
            .Include(c => c.FavoriteGadgets)
            .Where(g => g.CategoryId == categoryId && g.BrandId == brandId && g.Status == GadgetStatus.Active)
            .Select(c => c.ToGadgetResponse(currentUser != null ? currentUser.Customer!.Id : null))
            .ToPagedListAsync(request);

        return Ok(gadgets);
    }
}
