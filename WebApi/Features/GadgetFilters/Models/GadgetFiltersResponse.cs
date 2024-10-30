namespace WebApi.Features.GadgetFilters.Models;

public class GadgetFiltersResponse
{
    public string SpecificationKeyName { get; set; } = default!;
    public List<GadgetFilterResponse> GadgetFilters { get; set; } = [];
}
