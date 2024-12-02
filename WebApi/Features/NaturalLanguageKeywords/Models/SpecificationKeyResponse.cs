using WebApi.Features.Categories.Models;

namespace WebApi.Features.NaturalLanguageKeywords.Models;

public class SpecificationKeyResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public CategoryResponse Category { get; set; } = default!;
}
