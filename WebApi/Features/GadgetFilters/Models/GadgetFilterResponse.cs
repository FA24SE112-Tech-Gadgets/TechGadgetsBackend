namespace WebApi.Features.GadgetFilters.Models;

public class GadgetFilterResponse
{
    public Guid GadgetFilterId { get; set; }
    public string Value { get; set; } = default!;
}
