namespace WebApi.Data.Entities;

public class GadgetFilterOption
{
    public Guid Id { get; set; }
    public Guid GadgetFilterId { get; set; }
    public string Value { get; set; } = default!;

    public GadgetFilter GadgetFilter { get; set; } = default!;
}
