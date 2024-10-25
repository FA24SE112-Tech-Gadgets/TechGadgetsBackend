using WebApi.Data.Entities;

namespace WebApi.Features.Reviews.Models;

public class ReviewResponse
{
    public Guid Id { get; set; }
    public CustomerReviewResponse Customer { get; set; } = default!;
    public int Rating { get; set; }
    public string Content { get; set; } = default!;
    public bool IsPositive { get; set; }
    public ReviewStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsUpdated { get; set; }
}
