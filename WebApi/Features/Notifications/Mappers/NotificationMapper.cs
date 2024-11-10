using WebApi.Data.Entities;
using WebApi.Features.Notificaitons.Mappers;
using WebApi.Features.Notifications.Models;

namespace WebApi.Features.Notifications.Mappers;

public static class NotificationMapper
{
    public static NotificationResponse? ToNotificaitonResponse(this Notification n)
    {
        if (n != null)
        {
            return new NotificationResponse
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                IsRead = n.IsRead,
                Type = n.Type,
                SellerOrderId = n.SellerOrderId,
                CreatedAt = n.CreatedAt,
                Seller = n.User.Seller!.ToSellerNotiResponse(),
                Customer = n.User.Customer!.ToCustomerNotiResponse(),
            };
        }
        return null;
    }
}
