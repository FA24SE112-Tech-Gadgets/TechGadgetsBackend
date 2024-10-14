using WebApi.Data.Entities;

namespace WebApi.Features.FavoriteGadgets.Models;

public class UserFavoriteGadgetResponse
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
    public UserStatus Status { get; set; }
}
