using WebApi.Data.Entities;

namespace WebApi.Features.Carts.Models;

public class SellerCartResponse
{
    public Guid Id { get; set; }
    public string? CompanyName { get; set; }
    public string ShopName { get; set; } = default!;
    public string ShopAddress { get; set; } = default!;
    public BusinessModel BusinessModel { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public UserCartResponse User { get; set; } = default!;
}
