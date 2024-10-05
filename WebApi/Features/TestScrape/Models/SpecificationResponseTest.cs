namespace WebApi.Features.TestScrape.Models;

public class SpecificationResponseTest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<SpecificationKeyResponseTest> SpecificationKeys { get; set; } = [];
}
