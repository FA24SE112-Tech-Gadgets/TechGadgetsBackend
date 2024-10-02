using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Filters;
using WebApi.Features.Users.Mappers;
using WebApi.Features.Users.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Users;

[ApiController]
[JwtValidation]
public class GetCurrentUserController : ControllerBase
{
    [HttpGet("users/current")]
    [Tags("Users")]
    [SwaggerOperation(Summary = "Get Current User", Description = "This API is for getting the current authenticated user")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Handler([FromServices] CurrentUserService currentUserService)
    {
        var user = await currentUserService.GetCurrentUser();
        return Ok(user.ToUserResponse());
    }
}