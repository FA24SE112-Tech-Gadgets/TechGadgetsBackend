using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Cryptography;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Users;

[ApiController]
[JwtValidation]
[RequestValidation<Request>]
public class ChangeUserPassword : ControllerBase
{
    private const int SaltSize = 16; // 128 bit 
    private const int KeySize = 32;  // 256 bit
    private const int Iterations = 10000; // Number of PBKDF2 iterations
    public new record Request(
        string OldPassword,
        string NewPassword
    );

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.OldPassword)
                .NotEmpty()
                .WithMessage("Mật khẩu không được để trống")
                .MinimumLength(8)
                .WithMessage("Mật khẩu tối thiểu 8 kí tự");
            RuleFor(r => r.NewPassword)
                .NotEmpty()
                .WithMessage("Mật khẩu không được để trống")
                .MinimumLength(8)
                .WithMessage("Mật khẩu tối thiểu 8 kí tự")
                .Must((request, newPassword) => newPassword != request.OldPassword)
                .WithMessage("Mật khẩu mới phải khác mật khẩu cũ"); ;
        }
    }

    [HttpPut("user/change-password")]
    [Tags("Users")]
    [SwaggerOperation(
        Summary = "Change User Password",
        Description = "This API is for change user password" +
                            "<br>&nbsp; - User bị Inactive thì không thể đổi mật khẩu được."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromServices] AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        if (currentUser.LoginMethod == LoginMethod.Google)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("user", "Tài khoản này đăng nhập bằng Google")
                .Build();
        }

        if (!VerifyHashedPassword(currentUser!.Password!, request.OldPassword))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("password", "Mật khẩu không chính xác")
                .Build();
        }

        bool isOldPassword = request.OldPassword == request.NewPassword;
        if (isOldPassword)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("password", "Mật khẩu đã từng được sử dụng")
                .Build();
        }
        currentUser.Password = HashPassword(request.NewPassword);
        await context.SaveChangesAsync();

        return Ok("Đổi mật khẩu thành công");
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
