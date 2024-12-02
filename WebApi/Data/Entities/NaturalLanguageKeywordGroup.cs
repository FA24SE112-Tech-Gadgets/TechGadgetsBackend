namespace WebApi.Data.Entities;

public class NaturalLanguageKeywordGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public NaturalLanguageKeywordGroupStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<NaturalLanguageKeyword> NaturalLanguageKeywords { get; set; } = [];
    public ICollection<Criteria> Criteria { get; set; } = [];
}

public enum NaturalLanguageKeywordGroupStatus
{
    Active, Inactive
}