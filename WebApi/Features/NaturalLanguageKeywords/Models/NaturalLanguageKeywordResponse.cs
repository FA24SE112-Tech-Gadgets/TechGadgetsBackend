using WebApi.Data.Entities;

namespace WebApi.Features.NaturalLanguageKeywords.Models;

public class NaturalLanguageKeywordResponse
{
    public Guid Id { get; set; }
    public string Keyword { get; set; } = default!;
    public NaturalLanguageKeywordStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
