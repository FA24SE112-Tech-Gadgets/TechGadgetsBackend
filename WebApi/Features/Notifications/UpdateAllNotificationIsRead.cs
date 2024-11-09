using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
public class UpdateAllNotificationIsRead : ControllerBase
{
    [HttpPut("notification/all")]
    [Tags("Notifications")]
    [SwaggerOperation(
        Summary = "Update All Notification IsRead To True",
        Description = "API is for update all notificaiton isRead to true. Note:" +
                            "<br>&nbsp; - Dùng API này để đổi trạng thái tất cả thông báo thành đã xem." +
                            "<br>&nbsp; - Customer hay Seller cho dù bị khóa acc thì vẫn có thể dùng API này." +
                            "<br>&nbsp; - User bị Inactive thì vẫn có thể xem(cập nhật isRead) thông báo được."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        await context.Notifications
                    .Where(n => n.UserId == currentUser!.Id)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(n => n.IsRead, true));

        return Ok("Cập nhật thành công");
    }
}
