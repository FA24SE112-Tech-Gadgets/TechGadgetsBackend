namespace WebApi.Data.Entities;

public class SellerOrderItem
{
    public Guid Id { get; set; }
    public Guid SellerOrderId { get; set; }
    public Guid? GadgetDiscountId { get; set; }
    public int GadgetPrice { get; set; } = default!;
    public int GadgetQuantity { get; set; } = default!;
    public Guid GadgetId { get; set; }

    public GadgetDiscount? GadgetDiscount { get; set; }
    public SellerOrder SellerOrder { get; set; } = default!;
    public Gadget Gadget { get; set; } = default!;
    public Review? Review { get; set; }
}
