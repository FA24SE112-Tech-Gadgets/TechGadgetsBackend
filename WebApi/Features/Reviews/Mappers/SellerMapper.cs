using WebApi.Data.Entities;
using WebApi.Features.Reviews.Models;

namespace WebApi.Features.Reviews.Mappers;

public static class SellerMapper
{
    public static SellerResponse? ToSellerResponse(this Seller seller)
    {
        if (seller != null)
        {
            return new SellerResponse
            {
                Id = seller.Id,
                ShopName = seller.ShopName
            };
        }
        return null;
    }
}
