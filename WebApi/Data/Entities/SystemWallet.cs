namespace WebApi.Data.Entities;

public class SystemWallet
{
    public Guid Id { get; set; }
    public int Amount { get; set; }

    public ICollection<SystemOrderDetailTracking> SystemOrderDetailTrackings { get; set; } = [];
}
