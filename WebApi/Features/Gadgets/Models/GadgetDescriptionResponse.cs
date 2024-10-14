using WebApi.Data.Entities;

namespace WebApi.Features.Gadgets.Models;

public class GadgetDescriptionResponse
{
    public Guid Id { get; set; }
    public string Value { get; set; } = default!;
    public GadgetDescriptionType Type { get; set; }
    public int Index { get; set; }
}

