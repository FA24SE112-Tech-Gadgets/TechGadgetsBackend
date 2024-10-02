namespace WebApi.Data.Entities;

public class Specification
{
    public Guid Id { get; set; }
    public Guid GadgetId { get; set; }
    public string Name { get; set; } = default!;

    public Gadget Gadget { get; set; } = default!;
    public ICollection<SpecificationKey> SpecificationKeys { get; set; } = [];
}
