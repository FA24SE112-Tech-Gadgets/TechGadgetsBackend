using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Features.GadgetFilters.Mappers;
using WebApi.Features.GadgetFilters.Models;

namespace WebApi.Features.GadgetFilters;

[ApiController]
public class GetGadgetFilters : ControllerBase
{
    [HttpGet("gadget-filters/category/{categoryId}")]
    [Tags("Gadget Filters")]
    [SwaggerOperation(
        Summary = "Get List Gadget Filters By CategoryId",
        Description = "API is for get list of gadget filters by categoryId for display Filter."
    )]
    [ProducesResponseType(typeof(List<GadgetFiltersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context, [FromRoute] Guid categoryId)
    {
        var gadgetFiltersResponse = await context.SpecificationKeys
            .Where(sk => sk.CategoryId == categoryId && sk.GadgetFilters.Count > 0) // Filter by category and only keys with GadgetFilters
            .Include(sk => sk.GadgetFilters)
                .ThenInclude(gf => gf.SpecificationUnit)
            .Select(sk => new GadgetFiltersResponse
            {
                SpecificationKeyName = sk.Name,
                GadgetFilters = sk.GadgetFilters.ToListGadgetFilterResponse()!
            })
            .ToListAsync();

        return Ok(gadgetFiltersResponse);
    }
}
