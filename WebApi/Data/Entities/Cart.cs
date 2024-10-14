namespace WebApi.Data.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;
    public ICollection<CartGadget> CartGadgets { get; set; } = [];
}
