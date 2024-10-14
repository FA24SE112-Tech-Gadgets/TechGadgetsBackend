namespace WebApi.Features.Carts.Models;

public class CartGadgetItemResponse
{
    public Guid CartId { get; set; }
    public int Quantity { get; set; }
    public CartItemResponse Gadget { get; set; } = default!;
}
