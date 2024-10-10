using WebApi.Data.Entities;

namespace WebApi.Features.Sellers.Models;

public class UserDetailResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public Role Role { get; set; }
    public LoginMethod LoginMethod { get; set; }
    public UserStatus Status { get; set; }
}
