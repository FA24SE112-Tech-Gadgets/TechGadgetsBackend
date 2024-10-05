namespace WebApi.Services.Background.GadgetScrapeData.Models;

public class GadgetItemJsonResponse
{
    public int Price { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string NavigationUrl { get; set; } = default!;
}
