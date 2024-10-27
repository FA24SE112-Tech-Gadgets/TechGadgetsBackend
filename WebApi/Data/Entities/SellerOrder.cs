namespace WebApi.Data.Entities;

public class SellerOrder
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid SellerId { get; set; }
    public Guid CustomerInformationId { get; set; }
    public Guid SellerInformationId { get; set; }
    public SellerOrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public WalletTracking? WalletTracking { get; set; }
    public Order Order { get; set; } = default!;
    public Seller Seller { get; set; } = default!;
    public CustomerInformation CustomerInformation { get; set; } = default!;
    public SellerInformation SellerInformation { get; set; } = default!;
    public SystemSellerOrderTracking SystemOrderDetailTracking { get; set; } = default!;
    public ICollection<SellerOrderItem> SellerOrderItems { get; set; } = [];
}

public enum SellerOrderStatus
{
    Success, Pending, Cancelled
}
