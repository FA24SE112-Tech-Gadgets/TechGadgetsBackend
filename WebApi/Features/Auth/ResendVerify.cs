﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.VerifyCode;

namespace WebApi.Features.Auth;

[ApiController]
[RequestValidation<Request>]
public class ResendVerifyController : ControllerBase
{
    public new record Request(string Email);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Email không được để trống")
                .EmailAddress()
                .WithMessage("Email không hợp lệ");
        }
    }

    [HttpPost("auth/resend")]
    [Tags("Auth")]
    [SwaggerOperation(
        Summary = "Resend Verify Code",
        Description = "This API is for resending the verify code. Note:" +
                            "<br>&nbsp; - User bị Inactive thì vẫn resend verify code được (Vì liên quan đến tiền trong ví)."
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

        await verifyCodeService.ResendVerifyCodeAsync(user);

        return Ok("Đã gửi lại mã thành công.");
    }
}
