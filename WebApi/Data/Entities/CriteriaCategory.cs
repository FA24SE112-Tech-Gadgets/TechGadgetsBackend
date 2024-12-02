namespace WebApi.Data.Entities;

public class CriteriaCategory
{
    public Guid CategoryId { get; set; }
    public Guid CriteriaId { get; set; }

    public Category Category { get; set; } = default!;
    public Criteria Criteria { get; set; } = default!;
}
