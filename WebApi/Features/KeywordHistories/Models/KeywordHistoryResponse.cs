namespace WebApi.Features.KeywordHistories.Models;

public class KeywordHistoryResponse
{
    public Guid Id { get; set; }
    public string Keyword { get; set; } = default!;
}
