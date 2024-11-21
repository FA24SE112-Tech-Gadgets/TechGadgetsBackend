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
        var totalGadgetCount = await context.Gadgets.Where(g => g.SellerId == Guid.Parse("74231b9a-985a-47db-b589-d62c4ec16041")).CountAsync();

        return Ok(totalGadgetCount);
    }
}
