namespace WebApi.Data.Entities;

public class NaturalLanguagePrompt
{
    public Guid Id { get; set; }
    public string Prompt { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
