using WebApi.Data.Entities;
using WebApi.Features.SellerOrders.Models;

namespace WebApi.Features.SellerOrders.Mappers;

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
