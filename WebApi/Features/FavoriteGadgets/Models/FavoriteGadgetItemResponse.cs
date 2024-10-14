using WebApi.Data.Entities;

namespace WebApi.Features.FavoriteGadgets.Models;

public class FavoriteGadgetItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int Price { get; set; }
    public string ThumbnailUrl { get; set; } = default!;
    public GadgetStatus Status { get; set; }
    public string Condition { get; set; } = default!;
    public int Quantity { get; set; }
    public bool IsForSale { get; set; }
    public SellerFavoriteGadgetResponse Seller { get; set; } = default!;
    public BrandFavoriteGadgetResponse Brand { get; set; } = default!;
    public CategoryFavoriteGadgetResponse Category { get; set; } = default!;
}
