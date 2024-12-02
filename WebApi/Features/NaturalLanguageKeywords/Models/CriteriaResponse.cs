using WebApi.Data.Entities;
using WebApi.Features.Categories.Models;

namespace WebApi.Features.NaturalLanguageKeywords.Models;

public class CriteriaResponse
{
    public Guid Id { get; set; }
    public CriteriaType Type { get; set; }
    public SpecificationKeyResponse? SpecificationKey { get; set; }
    public string? Contains { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<CategoryResponse> Categories { get; set; } = [];
}