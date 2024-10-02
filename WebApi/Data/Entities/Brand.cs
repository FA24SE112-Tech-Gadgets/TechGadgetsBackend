namespace WebApi.Data.Entities;

public class Brand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;

    public ICollection<CategoryBrand> CategoryBrands { get; set; } = [];
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Gadget> Gadgets { get; set; } = [];
}
