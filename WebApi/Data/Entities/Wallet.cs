namespace WebApi.Data.Entities;

public class Wallet
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int Amount { get; set; }

    public User User { get; set; } = default!;
    public ICollection<WalletTracking> WalletTrackings { get; set; } = [];
}
