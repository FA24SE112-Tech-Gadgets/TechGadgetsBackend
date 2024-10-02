namespace WebApi.Data.Entities;

public class CustomerSubscription
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public CustomerSubscriptionType Type { get; set; }
    public int Price { get; set; }
    public int Duration { get; set; }
    public CustomerSubscriptionStatus Status { get; set; }

    public ICollection<CustomerSubscriptionTracker> CustomerSubscriptionTrackers { get; set; } = [];
}

public enum CustomerSubscriptionType
{
    Standard
}

public enum CustomerSubscriptionStatus
{
    Active, Inactive
}
