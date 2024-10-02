namespace WebApi.Data.Entities;

public class Seller
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? CompanyName { get; set; }
    public string ShopName { get; set; } = default!;
    public string ShippingAddress { get; set; } = default!;
    public string ShopAddress { get; set; } = default!;
    public BusinessModel BusinessModel { get; set; }
    public string? BusinessRegistrationCertificateUrl { get; set; }
    public string TaxCode { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public User User { get; set; } = default!;
    public ICollection<Gadget> Gadgets { get; set; } = [];
    public ICollection<SellerSubscriptionTracker> SellerSubscriptionTrackers { get; set; } = [];
    public ICollection<BillingMail> BillingMails { get; set; } = [];
    public ICollection<BannerRequest> BannerRequests { get; set; } = [];
}

public enum BusinessModel
{
    Personal, BusinessHousehold, Company
}