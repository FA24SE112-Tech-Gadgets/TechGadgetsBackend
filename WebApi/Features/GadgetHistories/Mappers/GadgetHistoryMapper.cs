using WebApi.Data.Entities;
using WebApi.Features.GadgetHistories.Models;

namespace WebApi.Features.GadgetHistories.Mappers;

public static class GadgetHistoryMapper
{
    private static SellerGadgetHistoryResponse? ToSellerGadgetHistoryResponse(this Seller seller)
    {
        if (seller != null)
        {
            return new SellerGadgetHistoryResponse
            {
                Id = seller.Id,
                CompanyName = seller.CompanyName,
                ShopName = seller.ShopName,
                ShopAddress = seller.ShopAddress,
                BusinessModel = seller.BusinessModel,
                PhoneNumber = seller.PhoneNumber,
                User = seller.User.ToUserGadgetHistoryResponse()!,
            };
        }
        return null;
    }

    private static UserGadgetHistoryResponse? ToUserGadgetHistoryResponse(this User user)
    {
        if (user != null)
        {
            return new UserGadgetHistoryResponse
            {
                Id = user.Id,
                Role = user.Role,
                Status = user.Status,
            };
        }
        return null;
    }

    private static CategoryGadgetHistoryResponse? ToCategoryGadgetHistoryResponse(this Category category)
    {
        if (category != null)
        {
            return new CategoryGadgetHistoryResponse
            {
                Id = category.Id,
                Name = category.Name,
            };
        }
        return null;
    }

    private static BrandGadgetHistoryResponse? ToBrandGadgetHistoryResponse(this Brand brand)
    {
        if (brand != null)
        {
            return new BrandGadgetHistoryResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                LogoUrl = brand.LogoUrl,
            };
        }
        return null;
    }

    private static GadgetHistoryItemResponse? ToGadgetHistoryItemResponse(this Gadget gadget)
    {
        if (gadget != null)
        {
            return new GadgetHistoryItemResponse
            {
                Id = gadget.Id,
                Name = gadget.Name,
                Price = gadget.Price,
                ThumbnailUrl = gadget.ThumbnailUrl,
                Status = gadget.Status,
                Condition = gadget.Condition,
                Quantity = gadget.Quantity,
                IsForSale = gadget.IsForSale,
                Seller = gadget.Seller.ToSellerGadgetHistoryResponse()!,
                Brand = gadget.Brand.ToBrandGadgetHistoryResponse()!,
                Category = gadget.Category.ToCategoryGadgetHistoryResponse()!,
            };
        }
        return null;
    }

    public static GadgetHistoryResponse? ToGadgetHistoryResponse(this GadgetHistory gadgetHistory)
    {
        if (gadgetHistory != null)
        {
            return new GadgetHistoryResponse
            {
                Id = gadgetHistory.Id,
                CreatedAt = gadgetHistory.CreatedAt,
                Gadget = gadgetHistory.Gadget.ToGadgetHistoryItemResponse()!,
            };
        }
        return null;
    }
}
