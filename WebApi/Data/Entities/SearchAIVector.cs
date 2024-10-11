using Pgvector;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.Entities;

public class SearchAIVector
{
    public Guid Id { get; set; }
    public Guid SearchAIId { get; set; }
    public string Value { get; set; } = default!;

    [Column(TypeName = "vector(384)")]
    public Vector Vector { get; set; } = default!;

    public SearchAI SearchAI { get; set; } = default!;
}
