using WebApi.Data.Entities;
using WebApi.Features.Notificaitons.Models;

namespace WebApi.Features.Notifications.Models;

public class NotificationResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool IsRead { get; set; }
    public NotificationType Type { get; set; }
    public Guid? SellerOrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public CustomerNotiResponse? Customer { get; set; } = default!;
    public SellerNotiResponse? Seller { get; set; } = default!;
}
