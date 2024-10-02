namespace WebApi.Data.Entities;

public class KeywordHistory
{
    public Guid Id { get; set; }
    public string Keyword { get; set; } = default!;
    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;
}
