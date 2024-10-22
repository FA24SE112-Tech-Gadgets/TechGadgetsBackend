using WebApi.Data.Entities;

namespace WebApi.Features.OrderDetails.Models;

public class OrderDetailResponse
{
    public OrderDetailStatus Status { get; set; }
    public string CustomerAddress { get; set; } = default!;
    public SellerOrderDetailResponse SellerInfo { get; set; } = default!;
    public int TotalQuantity { get; set; }
    public int TotalAmount { get; set; }
    public Guid OrderDetailId { get; set; }
    public string PaymentMethod { get; set; } = "Ví điện tử TechGadget";
    public DateTime OrderDetailCreatedAt { get; set; }
    public DateTime WalletTrackingCreatedAt { get; set; }
    public DateTime? OrderDetailUpdatedAt { get; set; } = default!;
}
