namespace WebApi.Data.Entities;

public class CartGadget
{
    public Guid CartId { get; set; }
    public Guid GadgetId { get; set; }
    public int Quantity { get; set; }

    public Cart Cart { get; set; } = default!;
    public Gadget Gadget { get; set; } = default!;
}
