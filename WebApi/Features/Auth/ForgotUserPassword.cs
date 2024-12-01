using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Cryptography;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.VerifyCode;

namespace WebApi.Features.Auth;

[ApiController]
[RolesFilter(Role.Customer, Role.Seller)]
[RequestValidation<Request>]
public class ForgotUserPassword : ControllerBase
{
    private const int SaltSize = 16; // 128 bit 
    private const int KeySize = 32;  // 256 bit
    private const int Iterations = 10000; // Number of PBKDF2 iterations
    public new record Request(
        string Email,
        string NewPassword,
        string Code
    );

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Email không được để trống")
                .EmailAddress()
                .WithMessage("Email không hợp lệ");
            RuleFor(r => r.NewPassword)
                .NotEmpty()
                .WithMessage("Mật khẩu không được để trống")
                .MinimumLength(8)
                .WithMessage("Mật khẩu tối thiểu 8 kí tự");
            RuleFor(r => r.Code)
                .NotEmpty()
                .WithMessage("Mã xác thực không được để trống");
        }
    }

    [HttpPost("auth/forgot-password")]
    [Tags("Auth")]
    [SwaggerOperation(
        Summary = "Forgot User Password",
        Description = "This API is for users that forgot password to change their password. Note:" +
                            "<br>&nbsp; - User bị Inactive thì vẫn forgot password được (Vì liên quan đến tiền trong ví)."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromServices] AppDbContext context, [FromServices] VerifyCodeService verifyCodeService)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Email == request.Email);
        if (user == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("user", "Người dùng không tồn tại")
                .Build();
        }

        if (user.LoginMethod == LoginMethod.Google)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("user", "Tài khoản này đăng nhập bằng Google")
                .Build();
        }

        await verifyCodeService.VerifyUserChangePasswordAsync(user, request.Code);
        user.Password = HashPassword(request.NewPassword);
        await context.SaveChangesAsync();

        return Ok("Cập nhật mật khẩu thành công!");
    }

    private static string HashPassword(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256);
        var salt = algorithm.Salt;
        var key = algorithm.GetBytes(KeySize);

        var hash = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hash, 0, SaltSize);
        Array.Copy(key, 0, hash, SaltSize, KeySize);

        return Convert.ToBase64String(hash);
    }
}
