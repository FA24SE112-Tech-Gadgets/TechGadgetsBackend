using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Features.Tests;

[ApiController]
public class TestEndpoint : ControllerBase
{
    [Tags("Tests")]
    [HttpPost("tests/endpoint")]
    public async Task<IActionResult> Handler(AppDbContext context)
    {
        var gadget = await context.Gadgets.FirstOrDefaultAsync();

        return Ok(gadget!.NameVector);
    }
}
