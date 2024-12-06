namespace WebApi.Features.NaturalLanguagePrompts.Models;

public class NaturalLanguagePromptResponse
{
    public Guid Id { get; set; }
    public string Prompt { get; set; } = default!;
}
