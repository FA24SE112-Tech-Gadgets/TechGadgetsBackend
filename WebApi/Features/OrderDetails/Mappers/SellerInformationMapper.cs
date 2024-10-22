using WebApi.Data.Entities;
using WebApi.Features.OrderDetails.Models;

namespace WebApi.Features.OrderDetails.Mappers;

public static class SellerInformationMapper
{
    public static SellerInfoResponse? ToSellerInfoResponse(this SellerInformation seller)
    {
        if (seller != null)
        {
            return new SellerInfoResponse
            {
                Id = seller.Id,
                ShopName = seller.ShopName,
                ShopAddress = seller.Address,
                PhoneNumber = seller.PhoneNumber,
            };
        }
        return null;
    }
}
