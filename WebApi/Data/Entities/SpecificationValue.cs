namespace WebApi.Data.Entities;

public class SpecificationValue
{
    public Guid Id { get; set; }
    public Guid SpecificationKeyId { get; set; }
    public string Value { get; set; } = default!;

    public SpecificationKey SpecificationKey { get; set; } = default!;
}
