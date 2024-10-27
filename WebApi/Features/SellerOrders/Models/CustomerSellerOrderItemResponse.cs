using WebApi.Data.Entities;

namespace WebApi.Features.SellerOrders.Models;

public class CustomerSellerOrderItemResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public int Amount { get; set; }
    public SellerInfoResponse SellerInfo { get; set; } = default!;
    public SellerOrderStatus Status { get; set; }
    public ICollection<SellerOrderItemInItemResponse> Gadgets { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
