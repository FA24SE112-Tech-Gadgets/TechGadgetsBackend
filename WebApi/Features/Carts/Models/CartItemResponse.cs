using WebApi.Data.Entities;

namespace WebApi.Features.Carts.Models;

public class CartItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int Price { get; set; }
    public string ThumbnailUrl { get; set; } = default!;
    public GadgetStatus Status { get; set; }
    public string Condition { get; set; } = default!;
    public int Quantity { get; set; }
    public bool IsForSale { get; set; }
    public BrandCartResponse Brand { get; set; } = default!;
    public CategoryCartResponse Category { get; set; } = default!;

}
