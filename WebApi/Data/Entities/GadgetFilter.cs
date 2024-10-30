using Pgvector;

namespace WebApi.Data.Entities;

public class GadgetFilter
{
    public Guid Id { get; set; }
    public Guid SpecificationKeyId { get; set; }
    public Guid? SpecificationUnitId { get; set; }
    public bool IsFilteredByVector { get; set; }
    public string Value { get; set; } = default!;
    public Vector Vector { get; set; } = default!;

    public SpecificationKey SpecificationKey { get; set; } = default!;
    public SpecificationUnit? SpecificationUnit { get; set; }
}
