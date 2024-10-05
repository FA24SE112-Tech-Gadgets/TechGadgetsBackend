namespace WebApi.Features.TestScrape.Models;

public class SellerResponseTest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? CompanyName { get; set; }
    public string ShopName { get; set; } = default!;
}
