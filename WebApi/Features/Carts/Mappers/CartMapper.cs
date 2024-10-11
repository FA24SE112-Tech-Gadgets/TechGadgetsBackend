using WebApi.Data.Entities;
using WebApi.Features.Carts.Models;

namespace WebApi.Features.Carts.Mappers;

public static class CartMapper
{
    public static ICollection<CartGadgetItemResponse>? ToCartListResponse(this ICollection<CartGadget> cartList)
    {
        if (cartList != null)
        {
            return cartList
            .Select(gadgetItem => new CartGadgetItemResponse
            {
                CartId = gadgetItem.CartId,
                GadgetId = gadgetItem.GadgetId,
                Quantity = gadgetItem.Quantity,
            })
            .ToList();
        }
        return null;
    }

    public static CartResponse? ToCartResponse(this Cart cart)
    {
        if (cart != null)
        {
            return new CartResponse
            {
                Id = cart.Id,
                CartGadgets = cart.CartGadgets.ToCartListResponse()!,
            };
        }
        return null;
    }
}
