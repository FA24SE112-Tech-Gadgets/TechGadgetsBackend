namespace WebApi.Features.Carts.Models;

public class CartGadgetItemResponse
{
    public Guid CartId { get; set; }
    public Guid GadgetId { get; set; }
    public int Quantity { get; set; }
}
