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
        var gadgets = await context.Gadgets.OrderByDescending(g => g.IsForSale).Select(g => g.IsForSale).ToListAsync();

        return Ok(gadgets);
    }
}
