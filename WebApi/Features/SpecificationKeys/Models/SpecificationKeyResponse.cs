namespace WebApi.Features.SpecificationKeys.Models;

public class SpecificationKeyResponse
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<SpecificationUnitResponse> SpecificationUnits { get; set; } = [];
}
