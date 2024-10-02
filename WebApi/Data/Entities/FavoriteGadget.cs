namespace WebApi.Data.Entities;

public class FavoriteGadget
{
    public Guid CustomerId { get; set; }
    public Guid GadgetId { get; set; }

    public Customer Customer { get; set; } = default!;
    public Gadget Gadget { get; set; } = default!;
}
