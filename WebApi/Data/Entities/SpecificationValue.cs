using Pgvector;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.Entities;

public class SpecificationValue
{
    public Guid Id { get; set; }
    public Guid SpecificationKeyId { get; set; }
    public Guid SpecificationUnitId { get; set; }
    public Guid GadgetId { get; set; }
    public string Value { get; set; } = default!;

    [Column(TypeName = "vector(384)")]
    public Vector Vector { get; set; } = default!;

    public SpecificationKey SpecificationKey { get; set; } = default!;
    public SpecificationUnit SpecificationUnit { get; set; } = default!;
    public Gadget Gadget { get; set; } = default!;
}
