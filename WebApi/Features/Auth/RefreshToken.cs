﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Features.Auth.Mappers;
using WebApi.Features.Auth.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Auth;

[ApiController]
[RequestValidation<Request>]
public class RefreshTokenController : ControllerBase
{
    public new record Request(string RefreshToken);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.RefreshToken)
                .NotEmpty()
                .WithMessage("Token không được để trống");
        }
    }

    [HttpPost("auth/refresh")]
    [Tags("Auth")]
    [SwaggerOperation(
        Summary = "Refresh Token", 
        Description = "This API is for refreshing a new token. Note:" +
                            "<br>&nbsp; - User bị Inactive thì vẫn gửi refreshToken được (Vì liên quan đến tiền trong ví)."
    )]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromServices] AppDbContext context, [FromServices] TokenService tokenService)
    {
        var userInfo = await tokenService.ValidateRefreshToken(request.RefreshToken, context);

        var tokenInfo = userInfo.ToTokenRequest();
        string token = tokenService.CreateToken(tokenInfo!);
        string refreshToken = tokenService.CreateRefreshToken(tokenInfo!);

        return Ok(new TokenResponse
        {
            Token = token,
            RefreshToken = refreshToken
        });
    }
}