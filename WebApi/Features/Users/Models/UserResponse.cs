using WebApi.Data.Entities;

namespace WebApi.Features.Users.Models;

public class UserResponse
{
    public Guid Id { get; set; }
    public LoginMethod LoginMethod { get; set; }
    public Role Role { get; set; }
    public string Email { get; set; } = default!;
    public UserStatus Status { get; set; }
    public ManagerResponse? Manager { get; set; }
    public AdminResponse? Admin { get; set; }
    public CustomerResponse? Customer { get; set; }
    public SellerResponse? Seller { get; set; }
}
