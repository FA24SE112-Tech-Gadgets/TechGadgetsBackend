namespace WebApi.Data.Entities;

public class KeywordHistory
{
    public Guid Id { get; set; }
    public string Keyword { get; set; } = default!;
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = default!;
}
