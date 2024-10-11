namespace WebApi.Data.Entities;

public class SearchAI
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
    public bool CanAddMore { get; set; }
    public string? Name { get; set; }

    public ICollection<SearchAIVector> SearchAIVectors { get; set; } = [];
}
