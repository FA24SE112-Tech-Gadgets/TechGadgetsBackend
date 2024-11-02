using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Notifications.Mappers;
using WebApi.Features.Notifications.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Notifications;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class GetNotificationsByCurrentUser : ControllerBase
{
    public new class Request : PagedRequest
    {
        public SortByDate? SortByDate { get; set; }
        public FilterBy? FilterBy { get; set; }
        public NotificationType? NotificationType { get; set; }
    }

    public enum SortByDate
    {
        DESC, ASC
    }

    public enum FilterBy
    {
       Read, Unread
    }

    [HttpGet("notifications")]
    [Tags("Notifications")]
    [SwaggerOperation(
        Summary = "Customer/Seller Get Their List Notifications",
        Description = "API is for get list of notificaitons base on Customer/Seller. Note:" +
                            "<br>&nbsp; - FilterBy: 'Read', 'Unread'. Không truyền tức là vừa 'Read' vừa 'Unread' luôn" +
                            "<br>&nbsp; - NotificationType: 'WalletTracking', 'SellerOrder'. Không truyền tức là vừa 'WalletTracking' vừa 'SellerOrder' luôn" +
                            "<br>&nbsp; - SortByDate: 'DESC', 'ASC'. Default: 'DESC' nếu không truyền" +
                            "<br>&nbsp; - Dùng API này sau khi nhận được notification từ Firebase Cloud Messaging(FCM), để lấy ra notification mới nhất bằng thứ tự DESC." +
                            "<br>&nbsp; - Customer hay Seller cho dù bị khóa acc thì vẫn có thể dùng API này."
    )]
    [ProducesResponseType(typeof(PagedList<NotificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var query = context.Notifications
            .Include(n => n.User)
                .ThenInclude(u => u.Customer)
            .Include(n => n.User)
                .ThenInclude(u => u.Seller)
            .Where(n => n.UserId == currentUser!.Id);

        // Filter by IsRead (Read, Unread, or both)
        if (request.FilterBy.HasValue)
        {
            query = request.FilterBy.Value switch
            {
                FilterBy.Read => query.Where(n => n.IsRead),
                FilterBy.Unread => query.Where(n => !n.IsRead),
                _ => query
            };
        }

        // Filter by NotificationType (WalletTracking, SellerOrder, or both)
        if (request.NotificationType.HasValue)
        {
            query = query.Where(n => n.Type == request.NotificationType.Value);
        }

        // Sort by Date (DESC by default)
        query = request.SortByDate == SortByDate.ASC
            ? query.OrderBy(n => n.CreatedAt)
            : query.OrderByDescending(n => n.CreatedAt);

        var notificaitonsResponse = await query
            .Select(n => n.ToNotificaitonResponse())
            .ToPagedListAsync(request);
        return Ok(notificaitonsResponse);
    }
}
