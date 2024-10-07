namespace WebApi.Data.Entities;

public class SellerApplication
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
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

    public User User { get; set; } = default!;
    public ICollection<BillingMailApplication> BillingMailApplications { get; set; } = [];
}

public enum SellerApplicationStatus
{
    Pending, Approved, Rejected
}
