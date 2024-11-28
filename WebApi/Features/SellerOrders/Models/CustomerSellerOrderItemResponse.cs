using WebApi.Data.Entities;

namespace WebApi.Features.SellerOrders.Models;

public class CustomerSellerOrderItemResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public long Amount { get; set; }
    public long DiscountAmount { get; set; }
    public long BeforeAppliedDiscountAmount { get; set; }
    public int TotalQuantity { get; set; }
    public SellerInfoResponse SellerInfo { get; set; } = default!;
    public SellerOrderStatus Status { get; set; }
    public ICollection<SellerOrderItemInItemResponse> Gadgets { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
