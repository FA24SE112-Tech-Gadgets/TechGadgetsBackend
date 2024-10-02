namespace WebApi.Data.Entities;

public class CustomerSubscriptionTracker
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid CustomerSubscriptionId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public int Amount { get; set; }
    public SubscriptionStatus SubscriptionStatus { get; set; }
    public DateTime? ValidityStart { get; set; }
    public DateTime? ValidityEnd { get; set; }
    public DateTime OverdueTime { get; set; }
    public DateTime CreatedAt { get; set; }

    public Customer Customer { get; set; } = default!;
    public CustomerSubscription CustomerSubscription { get; set; } = default!;
}

public enum PaymentMethod
{
    PayOS, Momo, VnPay
}

public enum PaymentStatus
{
    Unpaid, Paid
}

public enum SubscriptionStatus
{
    Active, Pending, Expired, Cancelled
}