using Pgvector;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.Entities;

public class Gadget
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public Guid BrandId { get; set; }
    public string Name { get; set; } = default!;
    public int Price { get; set; }
    public string ThumbnailUrl { get; set; } = default!;
    public Guid CategoryId { get; set; }
    public GadgetStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Condition { get; set; } = default!;

    [Column(TypeName = "vector(384)")]
    public Vector ConditionVector { get; set; } = default!;

    [Column(TypeName = "vector(384)")]
    public Vector NameVector { get; set; } = default!;
    public int Quantity { get; set; }

    public Seller Seller { get; set; } = default!;
    public Brand Brand { get; set; } = default!;
    public Category Category { get; set; } = default!;
    public ICollection<GadgetInformation> GadgetInformation { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<CartGadget> CartGadgets { get; set; } = [];
    public ICollection<GadgetDescription> GadgetDescriptions { get; set; } = [];
    public ICollection<SpecificationValue> SpecificationValues { get; set; } = [];
    public ICollection<GadgetImage> GadgetImages { get; set; } = [];
    public ICollection<FavoriteGadget> FavoriteGadgets { get; set; } = [];
    public ICollection<GadgetHistory> GadgetHistories { get; set; } = [];
    public ICollection<SearchGadgetResponse> SearchGadgetResponses { get; set; } = [];
}

public enum GadgetStatus
{
    Active, Inactive
}
