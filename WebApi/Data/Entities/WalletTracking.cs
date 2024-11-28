namespace WebApi.Data.Entities;

public class WalletTracking
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? SellerOrderId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public long Amount { get; set; }
    public long? BalanceBeforeChange { get; set; }
    public WalletTrackingType Type { get; set; }
    public WalletTrackingStatus Status { get; set; }
    public string? Reason { get; set; }
    public string? PaymentCode { get; set; }
    public DateTime? RefundedAt { get; set; }
    public DateTime? DepositedAt { get; set; }
    public DateTime? SellerPaidAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public Wallet Wallet { get; set; } = default!;
    public Order? Order { get; set; }
    public SellerOrder? SellerOrder { get; set; }
}

public enum PaymentMethod
{
    VnPay, Momo, PayOS
}

public enum WalletTrackingType
{
    Payment, Deposit, Refund, SellerTransfer
}

public enum WalletTrackingStatus
{
    Success, Pending, Expired, Failed, Cancelled
}
