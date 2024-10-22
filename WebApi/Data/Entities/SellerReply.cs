namespace WebApi.Data.Entities;

public class SellerReply
{
    public Guid Id { get; set; }
    public Guid ReviewId { get; set; }
    public Guid SellerId { get; set; }
    public string Content { get; set; } = default!;
    public SellerReplyStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Review Review { get; set; } = default!;
    public Seller Seller { get; set; } = default!;
}

public enum SellerReplyStatus
{
    Active, Inactive
}
