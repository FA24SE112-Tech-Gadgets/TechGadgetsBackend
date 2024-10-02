namespace WebApi.Data.Entities;

public class SpecificationKey
{
    public Guid Id { get; set; }
    public Guid? SpecificationId { get; set; }
    public Guid? GadgetId { get; set; }
    public string Name { get; set; } = default!;

    public Specification? Specification { get; set; }
    public Gadget? Gadget { get; set; }
    public ICollection<SpecificationValue> SpecificationValues { get; set; } = [];
}
