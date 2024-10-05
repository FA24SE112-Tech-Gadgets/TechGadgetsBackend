using WebApi.Data.Entities;

namespace WebApi.Features.TestScrape.Models;

public class SpecificationKeyResponseTest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<SpecificationValueResponseTest> SpecificationValues { get; set; } = [];
}
