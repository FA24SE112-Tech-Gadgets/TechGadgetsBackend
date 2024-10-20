using WebApi.Data.Entities;

namespace WebApi.Features.OrderDetails.Models;

public class CustomerOrderDetailItemResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public int Amount { get; set; }
    public SellerOrderDetailResponse Seller { get; set; } = default!;
    public OrderDetailStatus Status { get; set; }
    public ICollection<GadgetInformationOrderDetailResponse> GadgetInformation { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
