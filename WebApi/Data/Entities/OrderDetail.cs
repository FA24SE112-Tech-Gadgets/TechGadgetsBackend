namespace WebApi.Data.Entities;

public class OrderDetail
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid SellerId { get; set; }
    public int Amount { get; set; }
    public OrderDetailStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public WalletTracking? WalletTracking { get; set; }
    public Order Order { get; set; } = default!;
    public Seller Seller { get; set; } = default!;
    public SystemOrderDetailTracking SystemOrderDetailTracking { get; set; } = default!;
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<GadgetInformation> GadgetInformation { get; set; } = [];
}

public enum OrderDetailStatus
{
    Success, Pending, Cancelled
}
