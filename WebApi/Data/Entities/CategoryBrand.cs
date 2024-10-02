namespace WebApi.Data.Entities;

public class CategoryBrand
{
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }

    public Brand Brand { get; set; } = default!;
    public Category Category { get; set; } = default!;
}
