using WebApi.Data.Entities;

namespace WebApi.Features.Reviews.Models;

public class SellerReplyResponse
{
    public Guid Id { get; set; }
    public SellerResponse Seller { get; set; } = default!;
    public string Content { get; set; } = default!;
    public SellerReplyStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsUpdated { get; set; }
}
