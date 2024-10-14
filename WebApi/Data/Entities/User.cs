namespace WebApi.Data.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string? Password { get; set; }
    public Role Role { get; set; }
    public LoginMethod LoginMethod { get; set; }
    public UserStatus Status { get; set; }

    public Seller? Seller { get; set; }
    public Customer? Customer { get; set; }
    public Manager? Manager { get; set; }
    public Admin? Admin { get; set; }
    public Wallet? Wallet { get; set; }
    public ICollection<UserVerify> UserVerifies { get; set; } = [];
    public ICollection<SellerApplication> SellerApplications { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}

public enum Role
{
    Admin, Manager, Customer, Seller
}

public enum LoginMethod
{
    Default, Google
}

public enum UserStatus
{
    Active, Inactive, Pending
}