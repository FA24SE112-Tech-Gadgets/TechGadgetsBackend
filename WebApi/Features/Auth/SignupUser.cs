﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Cryptography;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.VerifyCode;

namespace WebApi.Features.Auth;

[ApiController]
[RequestValidation<Request>]
public class SignupUserController : ControllerBase
{
    public new record Request(string? FullName, string Email, string Password, Role Role, string? DeviceToken);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.FullName)
                .NotEmpty()
                .When(r => r.Role == Role.Customer)
                .WithMessage("Tên không được để trống");

            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Email không được để trống")
                .EmailAddress()
                .WithMessage("Email không đúng cú pháp");

            RuleFor(r => r.Password)
                .NotEmpty()
                .WithMessage("Mật khẩu không được để trống")
                .MinimumLength(8)
                .WithMessage("Mật khẩu tối thiểu 8 kí tự");

            RuleFor(r => r.Role)
                .Must(r => r == Role.Seller || r == Role.Customer)
                .WithMessage("Role phải là Seller hoặc Customer");
        }
    }

    [HttpPost("auth/signup")]
    [Tags("Auth")]
    [SwaggerOperation(
        Summary = "Signup User",
        Description = "This API is for user signup" +
                            "<br>&nbsp; - deviceToken: Dùng để gửi notification (mỗi 1 máy chỉ có duy nhất 1 deviceToken)." +
                            "<br>&nbsp; - 1 acc thì có thể được đăng nhập bằng nhiều thiết bị (điện thoại, laptop)." +
                            "<br>&nbsp; - deviceToken: Không gửi hoặc để trống cũng được, nhưng tức là thiết bị đó sẽ không nhận được notification thông qua FCM." +
                            "<br>&nbsp; - Hoặc sẽ nhận được notification nếu acc này đã lưu những deviceToken trước đó thì sẽ sẽ gửi noti đến những device đã đk vs acc này."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromServices] AppDbContext context, [FromServices] VerifyCodeService verifyCodeService)
    {
        var user = new User
        {
            Email = request.Email,
            Role = request.Role,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Pending,
            Password = HashPassword(request.Password),
        };

        if (user.Role == Role.Customer)
        {
            user.Customer = new Customer
            {
                FullName = request.FullName!,
            };
            Cart cart = new Cart
            {
                Customer = user!.Customer!,
            }!;
            await context.Carts.AddAsync(cart);
        }

        if (await context.Users.AnyAsync(us => us.Email == user.Email && us.Status == UserStatus.Pending))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("user", "Người dùng chưa xác thực tài khoản")
                .Build();
        }

        if (await context.Users.AnyAsync(us => us.Email == user.Email && us.Status != UserStatus.Pending))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_01)
                .AddReason("email", "Email này đã được đăng ký trước đó.")
                .Build();
        }

        if (!request.DeviceToken.IsNullOrEmpty())
        {
            user.Devices.Add(new Device
            {
                Token = request.DeviceToken!,
            });
        }

        context.Users.Add(user);
        await context.SaveChangesAsync();

        await verifyCodeService.SendVerifyCodeAsync(user);

        return Created();
    }

    private static string HashPassword(string password)
    {
        const int SaltSize = 16; // 128 bit 
        const int KeySize = 32;  // 256 bit
        const int Iterations = 10000; // Number of PBKDF2 iterations

        using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256);
        var salt = algorithm.Salt;
        var key = algorithm.GetBytes(KeySize);

        var hash = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hash, 0, SaltSize);
        Array.Copy(key, 0, hash, SaltSize, KeySize);

        return Convert.ToBase64String(hash);
    }
}
