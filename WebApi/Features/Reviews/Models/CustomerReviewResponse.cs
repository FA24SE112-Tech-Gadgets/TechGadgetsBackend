namespace WebApi.Features.Reviews.Models;

public class CustomerReviewResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string? AvatarUrl { get; set; }
}
