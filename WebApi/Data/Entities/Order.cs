namespace WebApi.Data.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;
    public WalletTracking WalletTracking { get; set; } = default!;
    public ICollection<OrderDetail> OrderDetails { get; set; } = [];
}
