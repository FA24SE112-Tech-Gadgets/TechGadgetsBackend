namespace WebApi.Data.Entities;

public class SpecificationUnit
{
    public Guid Id { get; set; }
    public Guid SpecificationKeyId { get; set; }
    public string Name { get; set; } = default!;

    public SpecificationKey SpecificationKey { get; set; } = default!;
    public ICollection<SpecificationValue> SpecificationValues { get; set; } = [];
}
