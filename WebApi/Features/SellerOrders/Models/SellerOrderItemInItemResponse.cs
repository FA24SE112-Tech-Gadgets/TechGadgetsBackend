using WebApi.Data.Entities;

namespace WebApi.Features.SellerOrders.Models;

public class SellerOrderItemInItemResponse
{
    public Guid SellerOrderItemId { get; set; }
    public Guid GadgetId { get; set; }
    public string Name { get; set; } = default!;
    public int Price { get; set; } = default!;
    public int DiscountPrice { get; set; } = default!;
    public int DiscountPercentage { get; set; }
    public string ThumbnailUrl { get; set; } = default!;
    public int Quantity { get; set; } = default!;
    public UserStatus SellerStatus { get; set; }
    public GadgetStatus GadgetStatus { get; set; }
}
