using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Notifications;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
[RequestValidation<Request>]
public class NotificationTestGroup(IHubContext<NotificationHub> hub) : ControllerBase
{
    public new class Request
    {
        public string TestMessasge { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(sp => sp.TestMessasge)
                .NotEmpty()
                .WithMessage("Tin nhắn không được để trống");
        }
    }

    [HttpPost("notification/group")]
    [Tags("Test Notifications")]
    [SwaggerOperation(
        Summary = "Test Sending Group Notification",
        Description = "This API is for testing sending group notification. Note: " +
                            "<br>&nbsp; - Dùng API này để test gửi notification theo group để FE nhận notification."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        switch (currentUser!.Role)
        {
            case Role.Admin:
            case Role.Manager:
                throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEA_01)
                .AddReason("notification", "Admin và Manager không có gửi notification.")
                .Build();
            case Role.Customer:
                await hub.Clients.Group("CustomerGroup").SendAsync("GroupMethod", request.TestMessasge);
                break;
            case Role.Seller:
                await hub.Clients.Group("SellerGroup").SendAsync("GroupMethod", request.TestMessasge);
                break;
            default:
                break;
        }

        return Ok();
    }
}
