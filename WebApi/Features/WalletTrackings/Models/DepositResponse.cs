namespace WebApi.Features.Wallets.Models;

public class DepositResponse
{
    public string DepositUrl { get; set; }
    public Guid WalletTrackingId { get; set; }
}
