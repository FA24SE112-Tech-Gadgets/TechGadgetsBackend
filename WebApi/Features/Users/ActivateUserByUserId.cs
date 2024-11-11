using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;
using WebApi.Services.Notifications;

namespace WebApi.Features.Users;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Admin)]
public class ActivateUserByUserId : ControllerBase
{
    [HttpPut("user/{userId}/activate")]
    [Tags("Users")]
    [SwaggerOperation(
        Summary = "Activate User",
        Description = "API for Admin to activate user. Note:" +
                            "<br>&nbsp; - User bị Inactive thì không thể active user khác được." +
                            "<br>&nbsp; - Có thể mở khóa cho cả role Admin khác nhưng làm gì có thể inactive role Admin đâu mà mở :)))."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid userId, AppDbContext context, [FromServices] CurrentUserService currentUserService, [FromServices] FCMNotificationService fcmNotificationService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        if (currentUser.Id == userId)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("admin", "Không thể tự mở khóa tài khoản của bản thân")
            .Build();
        }

        var user = await context.Users
            .Include(u => u.Devices)
            .FirstOrDefaultAsync(g => g.Id == userId);
        if (user is null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("user", "Người dùng không tồn tại")
            .Build();
        }

        if (user.Status == UserStatus.Active)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("user", $"Người dùng {userId} đã được mở khóa từ trước")
            .Build();
        }

        if (user.Status == UserStatus.Pending)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("user", $"Không thể mở khóa tài khoản chưa được kích hoạt")
            .Build();
        }

        DateTime createdAt = DateTime.UtcNow;
        try
        {
            string userTitle = $"Tài khoản của bạn đã được mở khóa";
            string userContent = $"Tài khoản của bạn đã được mở khóa sau khi TechGadget xem xét.";

            //Tạo thông báo cho seller
            List<string> deviceTokens = user.Devices.Select(d => d.Token).ToList();
            if (deviceTokens.Count > 0)
            {
                await fcmNotificationService.SendMultibleNotificationAsync(
                    deviceTokens,
                    userTitle,
                    userContent,
                    new Dictionary<string, string>()
                    {
                        { "userId", userId.ToString() },
                        { "notiType", NotificationType.User.ToString() },
                    }
                );
            }
            await context.Notifications.AddAsync(new Notification
            {
                UserId = userId,
                Title = userTitle,
                Content = userContent,
                CreatedAt = createdAt,
                IsRead = false,
                Type = NotificationType.User
            });

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        user.Status = UserStatus.Active;

        await context.SaveChangesAsync();

        return Ok("Mở khóa người dùng thành công");
    }
}
