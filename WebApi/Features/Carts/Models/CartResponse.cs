namespace WebApi.Features.Carts.Models;

public class CartResponse
{
    public Guid Id { get; set; }
    public ICollection<CartGadgetItemResponse> CartGadgets { get; set; } = [];
}
