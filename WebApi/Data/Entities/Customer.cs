namespace WebApi.Data.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string? AvatarUrl { get; set; }
    public string? Address { get; set; }
    public string? CCCD { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }

    public User User { get; set; } = default!;
    public ICollection<KeywordHistory> KeywordHistories { get; set; } = [];
    public ICollection<SearchHistory> SearchHistories { get; set; } = [];
    public ICollection<CustomerSubscriptionTracker> CustomerSubscriptionTrackers { get; set; } = [];
    public ICollection<GadgetHistory> GadgetHistories { get; set; } = [];
}

public enum Gender
{
    Male, Female
}
