using WebApi.Data.Entities;

namespace WebApi.Features.GadgetHistories.Models;

public class GadgetHistoryItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int Price { get; set; }
    public int DiscountPrice { get; set; } = default!;
    public int DiscountPercentage { get; set; }
    public DateTime? DiscountExpiredDate { get; set; }
    public string ThumbnailUrl { get; set; } = default!;
    public GadgetStatus Status { get; set; }
    public string Condition { get; set; } = default!;
    public int Quantity { get; set; }
    public bool IsForSale { get; set; }
    public bool IsFavorite { get; set; }
    public SellerGadgetHistoryResponse Seller { get; set; } = default!;
    public BrandGadgetHistoryResponse Brand { get; set; } = default!;
    public CategoryGadgetHistoryResponse Category { get; set; } = default!;
}
