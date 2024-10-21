using WebApi.Data.Entities;

namespace WebApi.Features.OrderDetails.Models;

public class SellerOrderDetailItemResponse
{
    public Guid Id { get; set; }
    public int Amount { get; set; }
    public OrderDetailStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
