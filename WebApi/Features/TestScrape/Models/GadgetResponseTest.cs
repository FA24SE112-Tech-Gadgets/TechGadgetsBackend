using WebApi.Data.Entities;

namespace WebApi.Features.TestScrape.Models;

public class GadgetResponseTest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int? Price { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Url { get; set; }
    public Guid? ShopId { get; set; }
    public GadgetStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public SellerResponseTest? Seller { get; set; }
    public BrandResponseTest? Brand { get; set; }
    public ShopResponseTest? Shop { get; set; }
    public CategoryResponseTest Category { get; set; } = default!;
    public ICollection<GadgetDescriptionResponseTest> GadgetDescriptions { get; set; } = [];
    public ICollection<SpecificationResponseTest> Specifications { get; set; } = [];
    public ICollection<SpecificationKeyResponseTest> SpecificationKeys { get; set; } = [];
    public ICollection<GadgetImageResponseTest> GadgetImages { get; set; } = [];
}
