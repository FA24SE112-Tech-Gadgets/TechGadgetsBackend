using WebApi.Data.Entities;
using WebApi.Features.Sellers.Models;

namespace WebApi.Features.Sellers.Mappers;

public static class SellerMapper
{
    public static ICollection<BillingMailResponse>? ToBillingMailListResponse(this ICollection<BillingMail> billingMails)
    {
        if (billingMails != null)
        {
            return billingMails
            .Select(email => new BillingMailResponse
            {
                Mail = email.Mail,
            })
            .ToList();
        }
        return null;
    }

    public static UserDetailResponse? ToUserDetailResponse(this User user)
    {
        if (user != null)
        {
            return new UserDetailResponse
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                LoginMethod = user.LoginMethod,
                Status = user.Status,
            };
        }
        return null;
    }

    public static SellerDetailResponse? ToSellerDetailResponse(this Seller seller)
    {
        if (seller != null)
        {
            return new SellerDetailResponse
            {
                Id = seller.Id,
                CompanyName = seller.CompanyName,
                ShopName = seller.ShopName,
                ShopAddress = seller.ShopAddress,
                BusinessModel = seller.BusinessModel,
                BusinessRegistrationCertificateUrl = seller.BusinessRegistrationCertificateUrl,
                TaxCode = seller.TaxCode,
                PhoneNumber = seller.PhoneNumber,
                User = seller.User.ToUserDetailResponse()!,
                BillingMails = seller.BillingMails.ToBillingMailListResponse()!,
            };
        }
        return null;
    }

    public static SellerResponse? ToSellerResponse(this Seller seller)
    {
        if (seller != null)
        {
            return new SellerResponse
            {
                Id = seller.Id,
                CompanyName = seller.CompanyName,
                ShopName = seller.ShopName,
                ShopAddress = seller.ShopAddress,
                BusinessModel = seller.BusinessModel,
                PhoneNumber = seller.PhoneNumber,
            };
        }
        return null;
    }
}
