using WebApi.Data.Entities;

namespace WebApi.Features.Orders.Mappers;

public static class OrderMapper
{
    public static GadgetInformation? ToGadgetInformation(this Gadget gadget)
    {
        if (gadget != null)
        {
            return new GadgetInformation
            {
                GadgetName = gadget.Name,
                GadgetThumbnailUrl = gadget.ThumbnailUrl,
                GadgetPrice = gadget.Price,
                GadgetId = gadget.Id,
            };
        }
        return null;
    }
}
