using WebApi.Data.Entities;

namespace WebApi.Features.NaturalLanguageKeywords.Models;

public class NaturalLanguageKeywordGroupResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public NaturalLanguageKeywordGroupStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<NaturalLanguageKeywordResponse> NaturalLanguageKeywords { get; set; } = [];
    public List<CriteriaResponse> Criteria { get; set; } = [];
}
