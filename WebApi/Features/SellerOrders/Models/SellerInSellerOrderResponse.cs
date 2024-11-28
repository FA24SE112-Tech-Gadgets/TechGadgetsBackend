using WebApi.Data.Entities;

namespace WebApi.Features.SellerOrders.Models;

public class SellerInSellerOrderResponse
{
    public SellerOrderStatus Status { get; set; }
    public CustomerInfoResponse CustomerInfo { get; set; } = default!;
    public SellerInfoResponse SellerInfo { get; set; } = default!;
    public int TotalQuantity { get; set; }
    public long TotalAmount { get; set; }
    public Guid SellerOrderId { get; set; }
    public string PaymentMethod { get; set; } = "Ví điện tử TechGadget";
    public DateTime SellerOrderCreatedAt { get; set; }
    public DateTime WalletTrackingCreatedAt { get; set; }
    public DateTime? SellerOrderUpdatedAt { get; set; } = default!;
    public string? CancelledReason { get; set; } = default!;
}
