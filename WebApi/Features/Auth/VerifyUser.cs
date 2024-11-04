using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Auth.Mappers;
using WebApi.Features.Auth.Models;
using WebApi.Services.Auth;
using WebApi.Services.VerifyCode;

namespace WebApi.Features.Auth;

[ApiController]
[RequestValidation<Request>]
public class VerifyUserController : ControllerBase
{
    public new record Request(string Email, string Code, string? DeviceToken);

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
    [SwaggerOperation(
        Summary = "Verify User", 
        Description = "This API is for verifying a user. Note:" +
                            "<br>&nbsp; - deviceToken: Dùng để gửi notification (mỗi 1 máy chỉ có duy nhất 1 deviceToken)." +
                            "<br>&nbsp; - 1 acc thì có thể được đăng nhập bằng nhiều thiết bị (điện thoại, laptop)." +
                            "<br>&nbsp; - deviceToken: Không gửi hoặc để trống cũng được, nhưng tức là thiết bị đó sẽ không nhận được notification thông qua FCM." +
                            "<br>&nbsp; - Hoặc sẽ nhận được notification nếu acc này đã lưu những deviceToken trước đó thì sẽ sẽ gửi noti đến những device đã đk vs acc này." +
                            "<br>&nbsp; - User bị Inactive thì vẫn Verify được (Vì liên quan đến tiền trong ví)."
    )]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Handler([FromBody] Request request,
        [FromServices] AppDbContext context, [FromServices] VerifyCodeService verifyCodeService, [FromServices] TokenService tokenService)
    {
        var user = await context.Users
            .Include(u => u.Devices)
            .FirstOrDefaultAsync(user => user.Email == request.Email);
        if (user == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("user", "Người dùng không tồn tại")
                .Build();
        }

        await verifyCodeService.VerifyUserAsync(user, request.Code);
        if (user.Role == Role.Customer || user.Role == Role.Seller)
        {
            Wallet wallet = new Wallet
            {
                User = user,
                Amount = 0
            };
            await context.Wallets.AddAsync(wallet);
            await context.SaveChangesAsync();
        }

        if (!request.DeviceToken.IsNullOrEmpty() && !user.Devices.Any(d => d.Token == request.DeviceToken))
        {
            context.Devices.Add(new Device
            {
                UserId = user.Id,
                Token = request.DeviceToken!,
            });
            await context.SaveChangesAsync();
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
}