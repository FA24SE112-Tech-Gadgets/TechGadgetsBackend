using WebApi.Data.Entities;
using WebApi.Features.FavoriteGadgets.Models;

namespace WebApi.Features.FavoriteGadgets.Mappers;

public static class FavoriteGadgetMapper
{
    public static FavoriteGadgetResponse? ToFavoriteGadgetResponse(this FavoriteGadget favoriteGadget)
    {
        if (favoriteGadget != null)
        {
            return new FavoriteGadgetResponse
            {
                CreatedAt = favoriteGadget.CreatedAt,
                Gadget = favoriteGadget.Gadget.ToFavoriteGadgetItemResponse()!
            };
        }
        return null;
    }

    private static FavoriteGadgetItemResponse? ToFavoriteGadgetItemResponse(this Gadget gadget)
    {
        if (gadget != null)
        {
            return new FavoriteGadgetItemResponse
            {
                Id = gadget.Id,
                Name = gadget.Name,
                Price = gadget.Price,
                ThumbnailUrl = gadget.ThumbnailUrl,
                Status = gadget.Status,
                Condition = gadget.Condition,
                Seller = gadget.Seller.ToSellerFavoriteGadgetResponse()!,
                Brand = gadget.Brand.ToBrandFavoriteGadgetResponse()!,
                Category = gadget.Category.ToCategoryFavoriteGadgetResponse()!,
            };
        }
        return null;
    }

    private static SellerFavoriteGadgetResponse? ToSellerFavoriteGadgetResponse(this Seller seller)
    {
        if (seller != null)
        {
            return new SellerFavoriteGadgetResponse
            {
                Id = seller.Id,
                CompanyName = seller.CompanyName,
                ShopName = seller.ShopName,
                ShopAddress = seller.ShopAddress,
                BusinessModel = seller.BusinessModel,
                PhoneNumber = seller.PhoneNumber,
                User = seller.User.ToFavoriteGadgetItemResponse()!,
            };
        }
        return null;
    }

    private static UserFavoriteGadgetResponse? ToFavoriteGadgetItemResponse(this User user)
    {
        if (user != null)
        {
            return new UserFavoriteGadgetResponse
            {
                Id = user.Id,
                Role = user.Role,
                Status = user.Status,
            };
        }
        return null;
    }

    private static BrandFavoriteGadgetResponse? ToBrandFavoriteGadgetResponse(this Brand brand)
    {
        if (brand != null)
        {
            return new BrandFavoriteGadgetResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                LogoUrl = brand.LogoUrl,
            };
        }
        return null;
    }

    private static CategoryFavoriteGadgetResponse? ToCategoryFavoriteGadgetResponse(this Category category)
    {
        if (category != null)
        {
            return new CategoryFavoriteGadgetResponse
            {
                Id = category.Id,
                Name = category.Name,
            };
        }
        return null;
    }
}
