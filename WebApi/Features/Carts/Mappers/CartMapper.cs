using WebApi.Data.Entities;
using WebApi.Features.Carts.Models;

namespace WebApi.Features.Carts.Mappers;

public static class CartMapper
{
    private static ICollection<CartGadgetItemResponse>? ToCartListResponse(this ICollection<CartGadget> cartList)
    {
        if (cartList != null)
        {
            return cartList
            .Select(gadgetItem => new CartGadgetItemResponse
            {
                CartId = gadgetItem.CartId,
                Quantity = gadgetItem.Quantity,
                Gadget = gadgetItem.Gadget.ToCartItemResponse()!,
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

    private static CartItemResponse? ToCartItemResponse(this Gadget gadget)
    {
        if (gadget != null)
        {
            return new CartItemResponse
            {
                Id = gadget.Id,
                Name = gadget.Name,
                Price = gadget.Price,
                ThumbnailUrl = gadget.ThumbnailUrl,
                Status = gadget.Status,
                Condition = gadget.Condition,
                Quantity = gadget.Quantity,
                IsForSale = gadget.IsForSale,
                Seller = gadget.Seller.ToSellerCartResponse()!,
                Brand = gadget.Brand.ToBrandcartResponse()!,
                Category = gadget.Category.ToCategoryCartResponse()!,
            };
        }
        return null;
    }

    private static SellerCartResponse? ToSellerCartResponse(this Seller seller)
    {
        if (seller != null)
        {
            return new SellerCartResponse
            {
                Id = seller.Id,
                CompanyName = seller.CompanyName,
                ShopName = seller.ShopName,
                ShopAddress = seller.ShopAddress,
                BusinessModel = seller.BusinessModel,
                PhoneNumber = seller.PhoneNumber,
                User = seller.User.ToUserCartResponse()!,
            };
        }
        return null;
    }

    private static UserCartResponse? ToUserCartResponse(this User user)
    {
        if (user != null)
        {
            return new UserCartResponse
            {
                Id = user.Id,
                Role = user.Role,
                Status = user.Status,
            };
        }
        return null;
    }

    private static BrandCartResponse? ToBrandcartResponse(this Brand brand)
    {
        if (brand != null)
        {
            return new BrandCartResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                LogoUrl = brand.LogoUrl,
            };
        }
        return null;
    }

    private static CategoryCartResponse? ToCategoryCartResponse(this Category category)
    {
        if (category != null)
        {
            return new CategoryCartResponse
            {
                Id = category.Id,
                Name = category.Name,
            };
        }
        return null;
    }

}
