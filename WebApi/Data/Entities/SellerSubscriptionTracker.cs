namespace WebApi.Data.Entities;

public class SellerSubscriptionTracker
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public Guid SellerSubscriptionId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public int Amount { get; set; }
    public int NumberOfMailLeft { get; set; }
    public SubscriptionStatus SubscriptionStatus { get; set; }
    public DateTime? ValidityStart { get; set; }
    public DateTime? ValidityEnd { get; set; }
    public DateTime OverdueTime { get; set; }
    public DateTime CreatedAt { get; set; }

    public Seller Seller { get; set; } = default!;
    public SellerSubscription SellerSubscription { get; set; } = default!;
}
