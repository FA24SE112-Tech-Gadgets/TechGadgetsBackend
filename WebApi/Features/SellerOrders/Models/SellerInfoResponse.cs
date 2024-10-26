namespace WebApi.Features.SellerOrders.Models;

public class SellerInfoResponse
{
    public Guid Id { get; set; }
    public string ShopName { get; set; } = default!;
    public string ShopAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}
