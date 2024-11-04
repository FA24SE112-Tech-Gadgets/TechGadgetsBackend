using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Notifications.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Notifications;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class UpdateNotificationIsRead : ControllerBase
{
    [HttpPut("notification/{notificationId}")]
    [Tags("Notifications")]
    [SwaggerOperation(
        Summary = "Update Notification IsRead To True By NotificationId",
        Description = "API is for update notificaiton isRead to true by notificationId. Note:" +
                            "<br>&nbsp; - Dùng API này để đổi trạng thái thành đã xem." +
                            "<br>&nbsp; - Customer hay Seller cho dù bị khóa acc thì vẫn có thể dùng API này." +
                            "<br>&nbsp; - User bị Inactive thì vẫn có thể xem(cập nhật isRead) thông báo được."
    )]
    [ProducesResponseType(typeof(PagedList<NotificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid notificationId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var notification = await context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId);
        if (notification == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("notification", "Không tìm thông báo này.")
            .Build();
        }

        if (notification.UserId != currentUser!.Id)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEA_01)
            .AddReason("notification", "Người dùng không đủ thẩm quyền để truy cập thông báo này.")
            .Build();
        }

        notification.IsRead = true;
        await context.SaveChangesAsync();
        
        return Ok("Cập nhật thành công");
    }
}
