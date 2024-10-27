namespace WebApi.Data.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<CategoryBrand> CategoryBrands { get; set; } = [];
    public ICollection<Brand> Brands { get; set; } = [];
    public ICollection<Gadget> Gadgets { get; set; } = [];
    public ICollection<SpecificationKey> SpecificationKeys { get; set; } = [];
}
