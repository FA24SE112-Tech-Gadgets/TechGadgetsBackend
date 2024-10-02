namespace WebApi.Data.Entities;

public class SearchHistory
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Message { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public Customer Customer { get; set; } = default!;
    public ICollection<SearchHistoryResponse> SearchHistoryResponses { get; set; } = [];
}
