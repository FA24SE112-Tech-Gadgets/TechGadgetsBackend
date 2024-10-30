using WebApi.Data.Entities;

namespace WebApi.Features.Orders.Mappers;

public static class OrderMapper
{
    public static SellerOrderItem? ToGadgetInformation(this Gadget gadget)
    {
        if (gadget != null)
        {
            return new SellerOrderItem
            {
                GadgetPrice = gadget.Price,
                GadgetId = gadget.Id,
                GadgetDiscount = gadget.GadgetDiscounts.FirstOrDefault(gd => gd.Status == GadgetDiscountStatus.Active),
            };
        }
        return null;
    }
}
