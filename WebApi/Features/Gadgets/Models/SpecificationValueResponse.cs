namespace WebApi.Features.Gadgets.Models;

public class SpecificationValueResponse
{
    public Guid Id { get; set; }
    public string SpecificationKey { get; set; } = default!;
    public string? SpecificationUnit { get; set; }
    public string Value { get; set; } = default!;
}
