using WebApi.Data.Entities;

namespace WebApi.Features.SellerApplications.Mappers;

public static class SellerApplicationMapper
{
    public static ICollection<BillingMailApplication>? ToBillingMailApplication(this ICollection<string> billingMailsRequest)
    {
        if (billingMailsRequest != null)
        {
            return billingMailsRequest
            .Select(email => new BillingMailApplication
            {
                Mail = email,
            })
            .ToList();
        }
        return null;
    }

    public static Seller? ToSeller(this SellerApplication sellerApplication)
    {
        if (sellerApplication != null)
        {
            return new Seller
            {
                UserId = sellerApplication.UserId,
                CompanyName = sellerApplication.CompanyName,
                ShopName = sellerApplication.ShopName,
                ShopAddress = sellerApplication.ShopAddress,
                BusinessModel = sellerApplication.BusinessModel,
                BusinessRegistrationCertificateUrl = sellerApplication.BusinessRegistrationCertificateUrl,
                TaxCode = sellerApplication.TaxCode,
                PhoneNumber = sellerApplication.PhoneNumber,
            };
        }
        return null;
    }
}
