using WebApi.Data.Entities;
using WebApi.Features.Gadgets.Models;

namespace WebApi.Features.Gadgets.Mappers;

public static class GadgetMapper
{
    public static GadgetResponse? ToGadgetResponse(this Gadget? gadget)
    {
        if (gadget != null)
        {
            return new GadgetResponse
            {
                Id = gadget.Id,
                Name = gadget.Name,
                Price = gadget.Price,
                SellerStatus = gadget.Seller.User.Status,
                ThumbnailUrl = gadget.ThumbnailUrl,
            };
        }
        return null;
    }
}
