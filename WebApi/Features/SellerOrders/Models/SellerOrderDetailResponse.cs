using WebApi.Data.Entities;

namespace WebApi.Features.SellerOrders.Models;

public class SellerOrderDetailResponse
{
    public SellerOrderStatus Status { get; set; }
    public CustomerInfoResponse CustomerInfo { get; set; } = default!;
    public SellerInfoResponse SellerInfo { get; set; } = default!;
    public int TotalQuantity { get; set; }
    public int TotalAmount { get; set; }
    public Guid OrderDetailId { get; set; }
    public string PaymentMethod { get; set; } = "Ví điện tử TechGadget";
    public DateTime OrderDetailCreatedAt { get; set; }
    public DateTime WalletTrackingCreatedAt { get; set; }
    public DateTime? OrderDetailUpdatedAt { get; set; } = default!;
    public string? CancelledReason { get; set; } = default!;
}
