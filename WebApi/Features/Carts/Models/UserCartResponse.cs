using WebApi.Data.Entities;

namespace WebApi.Features.Carts.Models;

public class UserCartResponse
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
    public UserStatus Status { get; set; }

}
