using WebApi.Data.Entities;

namespace WebApi.Features.SellerApplications.Models;

public class SellerApplicationDetailResponse
{
    public Guid Id { get; set; }
    public string? CompanyName { get; set; }
    public string ShopName { get; set; } = default!;
    public string ShopAddress { get; set; } = default!;
    public BusinessModel BusinessModel { get; set; }
    public string? BusinessRegistrationCertificateUrl { get; set; }
    public string TaxCode { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? RejectReason { get; set; }
    public SellerApplicationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<BillingMailApplicationResponse> BillingMailApplications { get; set; } = [];
}
