namespace WebApi.Data.Entities;

public class BannerRequest
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public Guid BannerRequestPriceId { get; set; }
    public string ImageUrl { get; set; } = default!;
    public DateTime ExpiredAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public Seller Seller { get; set; } = default!;
    public Banner Banner { get; set; } = default!;
    public BannerRequestPrice BannerRequestPrice { get; set; } = default!;
}
