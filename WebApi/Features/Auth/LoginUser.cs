using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Cryptography;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Auth.Mappers;
using WebApi.Features.Auth.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Auth;

[ApiController]
[RequestValidation<Request>]
public class LoginUserController : ControllerBase
{
    private const int SaltSize = 16; // 128 bit 
    private const int KeySize = 32;  // 256 bit
    private const int Iterations = 10000; // Number of PBKDF2 iterations

    public new record Request(string Email, string Password);
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Email không được để trống")
                .EmailAddress()
                .WithMessage("Email không hợp lệ");
            RuleFor(r => r.Password)
                .NotEmpty()
                .WithMessage("Mật khẩu không được để trống")
                .MinimumLength(8)
                .WithMessage("Mật khẩu phải có ít nhất 8 ký tự");
        }
    }

    [HttpPost("auth/login")]
    [Tags("Auth")]
    [SwaggerOperation(Summary = "Login User", Description = "This API is for user login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromServices] AppDbContext context, [FromServices] TokenService tokenService)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Email == request.Email);

        if (user == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("user", "Người dùng không tồn tại")
                .Build();
        }

        if (!VerifyHashedPassword(user.Password!, request.Password))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("password", "Mật khẩu không chính xác")
                .Build();
        }

        if (user.Status == UserStatus.Pending)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("user", "Người dùng chưa xác thực")
                .Build();
        }

        if (user.LoginMethod == LoginMethod.Google)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("user", "Tài khoản này đăng nhập bằng Google")
                .Build();
        }

        var tokenInfo = user.ToTokenRequest();
        string token = tokenService.CreateToken(tokenInfo!);
        string refreshToken = tokenService.CreateRefreshToken(tokenInfo!);

        return Ok(new TokenResponse
        {
            Token = token,
            RefreshToken = refreshToken
        });
    }

    private static bool VerifyHashedPassword(string hashedPassword, string passwordToCheck)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);

        var salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        using (var algorithm = new Rfc2898DeriveBytes(passwordToCheck, salt, Iterations, HashAlgorithmName.SHA256))
        {
            var keyToCheck = algorithm.GetBytes(KeySize);
            for (int i = 0; i < KeySize; i++)
            {
                if (hashBytes[i + SaltSize] != keyToCheck[i])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
