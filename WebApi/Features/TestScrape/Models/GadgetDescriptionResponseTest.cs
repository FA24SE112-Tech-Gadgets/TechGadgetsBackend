using WebApi.Data.Entities;

namespace WebApi.Features.TestScrape.Models;

public class GadgetDescriptionResponseTest
{
    public Guid Id { get; set; }
    public Guid GadgetId { get; set; }
    public string Value { get; set; } = default!;
    public GadgetDescriptionType Type { get; set; }
    public int Index { get; set; }
}
