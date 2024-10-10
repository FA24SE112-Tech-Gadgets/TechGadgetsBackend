using WebApi.Data.Entities;
using WebApi.Features.SellerApplications.Models;

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

    public static ICollection<BillingMail>? ToBillingMail(this ICollection<BillingMailApplication> billingMails)
    {
        if (billingMails != null)
        {
            return billingMails
            .Select(email => new BillingMail
            {
                Mail = email.Mail,
            })
            .ToList();
        }
        return null;
    }

    public static ICollection<BillingMailApplicationResponse>? ToBillingMailApplicationResponse(this ICollection<BillingMailApplication> billingMailsRequest)
    {
        if (billingMailsRequest != null)
        {
            return billingMailsRequest
            .Select(bm => new BillingMailApplicationResponse
            {
                Mail = bm.Mail,
            })
            .ToList();
        }
        return null;
    }

    public static Seller? ToSellerCreate(this SellerApplication sellerApplication)
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
                BillingMails = sellerApplication.BillingMailApplications.ToBillingMail()!,
            };
        }
        return null;
    }

    public static SellerApplicationDetailResponse? ToSellerApplicationDetailResponse(this SellerApplication sellerApplication)
    {
        if (sellerApplication != null)
        {
            return new SellerApplicationDetailResponse
            {
                Id = sellerApplication.Id,
                CompanyName = sellerApplication.CompanyName,
                ShopName = sellerApplication.ShopName,
                ShopAddress = sellerApplication.ShopAddress,
                BusinessModel = sellerApplication.BusinessModel,
                BusinessRegistrationCertificateUrl = sellerApplication.BusinessRegistrationCertificateUrl,
                TaxCode = sellerApplication.TaxCode,
                PhoneNumber = sellerApplication.PhoneNumber,
                RejectReason = sellerApplication.RejectReason,
                Status = sellerApplication.Status,
                CreatedAt = sellerApplication.CreatedAt,
                BillingMailApplications = sellerApplication.BillingMailApplications.ToBillingMailApplicationResponse()!,
            };
        }
        return null;
    }

    public static SellerApplicationItemResponse? ToSellerApplicationItemResponse(this SellerApplication sellerApplication)
    {
        if (sellerApplication != null)
        {
            return new SellerApplicationItemResponse
            {
                Id = sellerApplication.Id,
                CompanyName = sellerApplication.CompanyName,
                ShopName = sellerApplication.ShopName,
                ShopAddress = sellerApplication.ShopAddress,
                BusinessModel = sellerApplication.BusinessModel,
                PhoneNumber = sellerApplication.PhoneNumber,
                Status = sellerApplication.Status,
                CreatedAt = sellerApplication.CreatedAt,
            };
        }
        return null;
    }
}
