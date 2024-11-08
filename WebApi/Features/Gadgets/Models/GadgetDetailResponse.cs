using WebApi.Data.Entities;
using WebApi.Features.Brands.Models;
using WebApi.Features.Categories.Models;
using WebApi.Features.Users.Models;

namespace WebApi.Features.Gadgets.Models;

public class GadgetDetailResponse
{
    public Guid Id { get; set; }
    public SellerResponse Seller { get; set; } = default!;
    public BrandResponse Brand { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int Price { get; set; }
    public int DiscountPrice { get; set; } = default!;
    public int DiscountPercentage { get; set; }
    public DateTime? DiscountExpiredDate { get; set; }
    public string ThumbnailUrl { get; set; } = default!;
    public GadgetStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Condition { get; set; } = default!;
    public int Quantity { get; set; }
    public bool IsForSale { get; set; }
    public bool IsFavorite { get; set; }
    public UserStatus SellerStatus { get; set; }
    public CategoryResponse Category { get; set; } = default!;
    public ICollection<GadgetImageResponse> GadgetImages { get; set; } = [];
    public ICollection<GadgetDescriptionResponse> GadgetDescriptions { get; set; } = [];
    public ICollection<SpecificationValueResponse> SpecificationValues { get; set; } = [];

}
