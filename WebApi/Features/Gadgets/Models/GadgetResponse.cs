using WebApi.Data.Entities;

namespace WebApi.Features.Gadgets.Models;

public class GadgetResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int Price { get; set; }
    public string ThumbnailUrl { get; set; } = default!;
    public bool IsForSale { get; set; }
    public UserStatus SellerStatus { get; set; }
}
