using WebApi.Data.Entities;

namespace WebApi.Features.Users.Models;

public class SellerResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? CompanyName { get; set; }
    public string ShopName { get; set; } = default!;
    public string ShippingAddress { get; set; } = default!;
    public string ShopAddress { get; set; } = default!;
    public BusinessModel BusinessModel { get; set; }
    public string? BusinessRegistrationCertificateUrl { get; set; }
    public string TaxCode = default!;
    public string PhoneNumber = default!;
}
