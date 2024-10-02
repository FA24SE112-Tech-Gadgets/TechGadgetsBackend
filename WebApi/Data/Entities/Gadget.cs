﻿namespace WebApi.Data.Entities;

public class Gadget
{
    public Guid Id { get; set; }
    public Guid? SellerId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = default!;
    public int? Price { get; set; }
    public string? ThumbnailUrl { get; set; }
    public Guid CategoryId { get; set; }
    public string? Url { get; set; }
    public Guid? ShopId { get; set; }
    public GadgetStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Seller? Seller { get; set; }
    public Gadget? Parent { get; set; }
    public Brand? Brand { get; set; }
    public Shop? Shop { get; set; }
    public Category Category { get; set; } = default!;
    public ICollection<Gadget> Children { get; set; } = [];
    public ICollection<GadgetDescription> GadgetDescriptions { get; set; } = [];
    public ICollection<Specification> Specifications { get; set; } = [];
    public ICollection<SpecificationKey> SpecificationKeys { get; set; } = [];
    public ICollection<GadgetImage> GadgetImages { get; set; } = [];
    public ICollection<FavoriteGadget> FavoriteGadgets { get; set; } = [];
    public ICollection<GadgetHistory> GadgetHistories { get; set; } = [];
    public ICollection<SearchGadgetResponse> SearchGadgetResponses { get; set; } = [];
}

public enum GadgetStatus
{
    Active, Inactive
}
