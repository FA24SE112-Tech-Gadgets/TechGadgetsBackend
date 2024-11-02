using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Data.Entities;
using WebApi.Data;
using WebApi.Services.Auth;
using WebApi.Common.Filters;
using WebApi.Services.Notifications;

namespace WebApi.Features.Notifications;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
[RequestValidation<Request>]
public class NotificationTestCurrentNew : ControllerBase
{
    public new class Request
    {
        public string TestTitle { get; set; } = default!;
        public string TestContent { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(sp => sp.TestTitle)
                .NotEmpty()
                .WithMessage("Tiêu đề không được để trống");
            RuleFor(sp => sp.TestContent)
                .NotEmpty()
                .WithMessage("Nội dung không được để trống");
        }
    }

    [HttpPost("notification/new/current")]
    [Tags("Test Notifications")]
    [SwaggerOperation(
        Summary = "Test Sending Personal Notification",
        Description = "This API is for testing sending personal notification. Note: " +
                            "<br>&nbsp; - Dùng API này để test gửi personal notification để FE nhận notification."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService, [FromServices] FCMNotificationService fcmNotificationService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        try
        {
            List<string> deviceTokens = currentUser!.Devices.Select(d => d.Token).ToList();
            if (deviceTokens.Count > 0) {
                await fcmNotificationService.SendMultibleNotificationAsync(deviceTokens, request.TestTitle, request.TestContent, new Dictionary<string, string>()
                {
                    { "testData1", "testValue" },
                    { "testData2", "200" },
                });
            }

        }
        catch (Exception ex)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("notification", ex.Message)
            .Build();
        }

        return Ok("Send Success");
    }
}
