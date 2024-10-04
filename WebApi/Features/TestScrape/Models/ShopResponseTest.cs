namespace WebApi.Features.TestScrape.Models;

public class ShopResponseTest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string WebsiteUrl { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;
}
