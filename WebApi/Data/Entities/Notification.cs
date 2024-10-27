namespace WebApi.Data.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool IsSent { get; set; }
    public bool IsRead { get; set; }
    public NotificationType Type { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = default!;
}

public enum NotificationType
{
    WalletTracking, SellerOrder
}
