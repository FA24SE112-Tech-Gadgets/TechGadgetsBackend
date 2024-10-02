namespace WebApi.Data.Entities;

public class SearchHistoryResponse
{
    public Guid Id { get; set; }
    public Guid SearchHistoryId { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }

    public SearchHistory SearchHistory { get; set; } = default!;
    public ICollection<SearchGadgetResponse> SearchGadgetResponses { get; set; } = [];
}
