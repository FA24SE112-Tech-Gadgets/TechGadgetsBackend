using WebApi.Data.Entities;

namespace WebApi.Features.Gadgets.Models;

public class GadgetRelatedToSellerResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int Price { get; set; }
    public int DiscountPrice { get; set; } = default!;
    public int DiscountPercentage { get; set; }
    public DateTime? DiscountExpiredDate { get; set; }
    public string ThumbnailUrl { get; set; } = default!;
    public bool IsForSale { get; set; }
    public GadgetStatus GadgetStatus { get; set; }
}
