using WebApi.Data.Entities;
using WebApi.Features.Users.Models;

namespace WebApi.Features.Users.Mappers;

public static class SellerMapper
{
    public static SellerResponse? ToSellerResponse(this Seller? seller)
    {
        if (seller != null)
        {
            return new SellerResponse
            {
                Id = seller.Id,
                PhoneNumber = seller.PhoneNumber,
                BusinessModel = seller.BusinessModel,
                BusinessRegistrationCertificateUrl = seller.BusinessRegistrationCertificateUrl,
                CompanyName = seller.CompanyName,
                ShopAddress = seller.ShopAddress,
                ShopName = seller.ShopName,
                TaxCode = seller.TaxCode,
                UserId = seller.UserId
            };
        }
        return null;
    }
}
