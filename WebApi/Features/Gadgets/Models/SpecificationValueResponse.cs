namespace WebApi.Features.Gadgets.Models;

public class SpecificationValueResponse
{
    public Guid Id { get; set; }
    public SpecificationKeyResponse SpecificationKey { get; set; } = default!;
    public SpecificationUnitResponse? SpecificationUnit { get; set; }
    public string Value { get; set; } = default!;
}
