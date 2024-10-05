namespace WebApi.Services.Background.GadgetScrapeData.Models;

public class GadgetFPTShopItemJsonResponse
{
    public string? Price { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string NavigationUrl { get; set; } = default!;
}
