namespace WebApi.Data.Entities;

public class BannerRequestPrice
{
    public Guid Id { get; set; }
    public int Duration { get; set; }
    public string Name { get; set; } = default!;
    public int Price { get; set; }
    public BannerRequestPriceStatus Status { get; set; }

    public ICollection<BannerRequest> BannerRequests { get; set; } = [];
}

public enum BannerRequestPriceStatus
{
    Active, Inactive
}
