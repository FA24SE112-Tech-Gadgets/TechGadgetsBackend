using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.Entities;

public class SystemSellerOrderTracking
{
    public Guid Id { get; set; }
    public Guid SystemWalletId { get; set; }
    public Guid SellerOrderId { get; set; }

    [ForeignKey(nameof(FromUser))]
    public Guid FromUserId { get; set; }

    [ForeignKey(nameof(ToUser))]
    public Guid ToUserId { get; set; }

    public SystemSellerOrderTrackingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public SystemWallet SystemWallet { get; set; } = default!;
    public SellerOrder SellerOrder { get; set; } = default!;
    public User FromUser { get; set; } = default!;
    public User ToUser { get; set; } = default!;
}

public enum SystemSellerOrderTrackingStatus
{
    Refunded, Paid, Pending
}
