namespace WebApi.Features.Wallets.Models;

public class DepositResponse
{
    public string DepositUrl { get; set; } = default!;
    public Guid WalletTrackingId { get; set; }
}
