using WebApi.Data.Entities;

namespace WebApi.Features.OrderDetails.Models;

public class CustomerOrderDetailItemResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public int Amount { get; set; }
    public SellerInfoResponse SellerInfo { get; set; } = default!;
    public OrderDetailStatus Status { get; set; }
    public ICollection<GadgetInformationOrderDetailResponse> Gadgets { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
