namespace WebApi.Data.Entities;

public class Banner
{
    public Guid Id { get; set; }
    public Guid? BannerRequestId { get; set; }
    public string ImageUrl { get; set; } = default!;
    public BannerStatus Status { get; set; }

    public BannerRequest? BannerRequest { get; set; }
}

public enum BannerStatus
{
    Active, Inactive
}
