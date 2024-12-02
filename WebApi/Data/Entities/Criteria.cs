namespace WebApi.Data.Entities;

public class Criteria
{
    public Guid Id { get; set; }
    public Guid NaturalLanguageKeywordGroupId { get; set; }
    public CriteriaType Type { get; set; }
    public Guid? SpecificationKeyId { get; set; }
    public string? Contains { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public NaturalLanguageKeywordGroup NaturalLanguageKeywordGroup { get; set; } = default!;
    public SpecificationKey? SpecificationKey { get; set; }
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<CriteriaCategory> CriteriaCategories { get; set; } = [];
}

public enum CriteriaType
{
    Name, Description, Condition, Price, Specification
}
