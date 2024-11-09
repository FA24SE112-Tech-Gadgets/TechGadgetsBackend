using WebApi.Data.Entities;

namespace WebApi.Features.WalletTrackings.Models;

public class WalletTrackingItemResponse
{
    public Guid Id { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public int Amount { get; set; }
    public WalletTrackingType Type { get; set; }
    public WalletTrackingStatus Status { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? SellerOrderId { get; set; }
    public DateTime? RefundedAt { get; set; }
    public DateTime? DepositedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
