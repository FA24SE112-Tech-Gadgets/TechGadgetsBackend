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
    public Cart Cart { get; set; } = default!;
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<GadgetHistory> GadgetHistories { get; set; } = [];
    public ICollection<FavoriteGadget> FavoriteGadgets { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<CustomerInformation> CustomerInformation { get; set; } = [];
}

public enum Gender
{
    Male, Female
}
