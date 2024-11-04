using WebApi.Data.Entities;
using WebApi.Features.Notificaitons.Models;

namespace WebApi.Features.Notificaitons.Mappers;

public static class SellerNotiMapper
{
    public static SellerNotiResponse? ToSellerNotiResponse(this Seller seller)
    {
        if (seller != null)
        {
            return new SellerNotiResponse
            {
                Id = seller.Id,
                ShopName = seller.ShopName,
            };
        }
        return null;
    }
}
