namespace WebApi.Features.Carts.Models;

public class CartGadgetItemResponse
{
    public int Quantity { get; set; }
    public CartItemResponse Gadget { get; set; } = default!;
}
