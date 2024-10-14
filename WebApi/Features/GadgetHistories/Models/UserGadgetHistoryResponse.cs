using WebApi.Data.Entities;

namespace WebApi.Features.GadgetHistories.Models;

public class UserGadgetHistoryResponse
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
    public UserStatus Status { get; set; }
}
