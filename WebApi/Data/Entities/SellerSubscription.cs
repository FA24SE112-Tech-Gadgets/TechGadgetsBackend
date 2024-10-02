namespace WebApi.Data.Entities;

public class SellerSubscription
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public SellerSubscriptionType Type { get; set; }
    public int Price { get; set; }
    public int Duration { get; set; }
    public SellerSubscriptionStatus Status { get; set; }

    public ICollection<SellerSubscriptionTracker> SellerSubscriptionTrackers { get; set; } = [];
}

public enum SellerSubscriptionType
{
    Standard, Premium
}

public enum SellerSubscriptionStatus
{
    Active, Inactive
}