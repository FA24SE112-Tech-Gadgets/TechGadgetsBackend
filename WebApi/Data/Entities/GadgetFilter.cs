namespace WebApi.Data.Entities;

public class GadgetFilter
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = default!;

    public Category Category { get; set; } = default!;
    public ICollection<GadgetFilterOption> GadgetFilterOptions { get; set; } = [];
}
