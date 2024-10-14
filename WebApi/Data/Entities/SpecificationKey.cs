namespace WebApi.Data.Entities;

public class SpecificationKey
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = default!;

    public Category Category { get; set; } = default!;
    public ICollection<SpecificationValue> SpecificationValues { get; set; } = [];
    public ICollection<SpecificationUnit> SpecificationUnits { get; set; } = [];
}
