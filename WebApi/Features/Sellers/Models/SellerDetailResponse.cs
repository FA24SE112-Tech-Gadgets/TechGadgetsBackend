using WebApi.Data.Entities;

namespace WebApi.Features.Sellers.Models;

public class SellerDetailResponse
{
    public Guid Id { get; set; }
    public string? CompanyName { get; set; }
    public string ShopName { get; set; } = default!;
    public string ShopAddress { get; set; } = default!;
    public BusinessModel BusinessModel { get; set; }
    public string? BusinessRegistrationCertificateUrl { get; set; }
    public string TaxCode { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public UserDetailResponse User { get; set; } = default!;
    public ICollection<BillingMailResponse> BillingMails { get; set; } = [];
}
