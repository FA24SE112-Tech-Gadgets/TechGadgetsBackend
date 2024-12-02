namespace WebApi.Data.Entities;

public class NaturalLanguageKeyword
{
    public Guid Id { get; set; }
    public Guid NaturalLanguageKeywordGroupId { get; set; }
    public string Keyword { get; set; } = default!;
    public NaturalLanguageKeywordStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public NaturalLanguageKeywordGroup NaturalLanguageKeywordGroup { get; set; } = default!;
}

public enum NaturalLanguageKeywordStatus
{
    Active, Inactive
}