using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Features.Auth.Mappers;
using WebApi.Features.Auth.Models;
using WebApi.Services.Auth;
using WebApi.Services.VerifyCode;

namespace WebApi.Features.Auth;

[ApiController]
[RequestValidation<Request>]
public class VerifyUserController : ControllerBase
{
    public new record Request(string Email, string Code);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Email không được để trống")
                .EmailAddress()
                .WithMessage("Email không hợp lệ");

            RuleFor(r => r.Code)
                .NotEmpty()
                .WithMessage("Mã xác thực không được để trống");
        }
    }

    [HttpPost("auth/verify")]
    [Tags("Auth")]
    [SwaggerOperation(Summary = "Verify User", Description = "This API is for verifying a user")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Handler([FromBody] Request request,
        [FromServices] AppDbContext context, [FromServices] VerifyCodeService verifyCodeService, [FromServices] TokenService tokenService)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Email == request.Email);
        if (user == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("user", "Người dùng không tồn tại")
                .Build();
        }

        await verifyCodeService.VerifyUserAsync(user, request.Code);

        var tokenInfo = user.ToTokenRequest();
        string token = tokenService.CreateToken(tokenInfo!);
        string refreshToken = tokenService.CreateRefreshToken(tokenInfo!);

        return Ok(new TokenResponse
        {
            Token = token,
            RefreshToken = refreshToken
        });
    }
}