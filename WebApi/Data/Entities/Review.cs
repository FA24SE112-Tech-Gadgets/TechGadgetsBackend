namespace WebApi.Data.Entities;

public class Review
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid SellerOrderItemId { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; } = default!;
    public bool IsPositive { get; set; }
    public ReviewStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public SellerOrderItem SellerOrderItem { get; set; } = default!;
    public Customer Customer { get; set; } = default!;
    public SellerReply? SellerReply { get; set; }
}

public enum ReviewStatus
{
    Active, Inactive
}
