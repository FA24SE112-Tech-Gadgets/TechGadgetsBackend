using Microsoft.AspNetCore.Mvc;
using WebApi.Data;

namespace WebApi.Features.Tests;

[ApiController]
public class TestEndpoint : ControllerBase
{
    [Tags("Tests")]
    [HttpPost("tests/endpoint")]
    public async Task<IActionResult> Handler(AppDbContext context)
    {
        var i = 1 - 5 / 100.0;

        return Ok(i);
    }
}
