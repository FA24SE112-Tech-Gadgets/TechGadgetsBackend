namespace WebApi.Data.Entities;

public class SellerInformation
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public Guid OrderDetailId { get; set; }
    public string ShopName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public Seller Seller { get; set; } = default!;
    public OrderDetail OrderDetail { get; set; } = default!;
}
