using Microsoft.AspNetCore.Mvc;

namespace WebApi.Features.Tests;

[ApiController]
public class TestEndpoint : ControllerBase
{
    [Tags("Tests")]
    [HttpPost("tests/endpoint")]
    public async Task<IActionResult> Handler()
    {
        await Task.Delay(20000);

        return Ok();
    }
}
