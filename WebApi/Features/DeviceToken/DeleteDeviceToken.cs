using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Services.Auth;

namespace WebApi.Features.DeviceToken;

[ApiController]
[JwtValidation]
[RequestValidation<Request>]
public class DeleteDeviceToken : ControllerBase
{
    public new class Request
    {
        public string Token { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Token)
                .NotEmpty().WithMessage("Token không được để trống");
        }
    }

    [HttpDelete("device-tokens")]
    [Tags("Device Tokens")]
    [SwaggerOperation(
        Summary = "User Delete Device Token When Logout"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Request request, AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (!await context.Devices.AnyAsync(dv => dv.User == currentUser && dv.Token == request.Token))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("device", "Không tìm thấy device token.")
                .Build();
        }

        await context.Devices
                        .Where(dv => dv.User == currentUser && dv.Token == request.Token)
                        .ExecuteDeleteAsync();

        return Ok("Xóa Device Token thành công");
    }
}
