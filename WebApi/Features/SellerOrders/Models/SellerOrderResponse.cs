using WebApi.Data.Entities;

namespace WebApi.Features.SellerOrders.Models;

public class SellerOrderResponse
{
    public Guid Id { get; set; }
    public CustomerInfoResponse Customer { get; set; } = default!;
    public int Amount { get; set; }
    public SellerOrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
