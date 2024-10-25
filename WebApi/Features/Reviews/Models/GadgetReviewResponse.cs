using WebApi.Data.Entities;

namespace WebApi.Features.Reviews.Models;

public class GadgetReviewResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string ThumbnailUrl { get; set; } = default!;
    public GadgetStatus Status { get; set; }
}
