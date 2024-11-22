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
public class GetNumberOfGadgetsByManager : ControllerBase
{
    [HttpGet("stats/number-of-gadgets/manager")]
    [Tags("Stats")]
    [SwaggerOperation(
        Summary = "Manager Gets Number Of Gadgets",
        Description = "API is for manager gets number of gadgets"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context)
    {
        var response = await context.Gadgets.CountAsync();

        return Ok(new
        {
            numberOfGadgets = response
        });
    }
}
