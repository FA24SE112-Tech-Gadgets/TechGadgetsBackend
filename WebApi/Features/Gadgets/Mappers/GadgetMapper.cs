using WebApi.Data.Entities;
using WebApi.Features.Brands.Mappers;
using WebApi.Features.Categories.Mappers;
using WebApi.Features.Gadgets.Models;
using WebApi.Features.Users.Mappers;

namespace WebApi.Features.Gadgets.Mappers;

public static class GadgetMapper
{
    public static GadgetResponse? ToGadgetResponse(this Gadget? gadget)
    {
        if (gadget != null)
        {
            return new GadgetResponse
            {
                Id = gadget.Id,
                Name = gadget.Name,
                Price = gadget.Price,
                SellerStatus = gadget.Seller.User.Status,
                ThumbnailUrl = gadget.ThumbnailUrl,
                IsForSale = gadget.IsForSale,
            };
        }
        return null;
    }

    public static GadgetDetailResponse? ToGadgetDetailResponse(this Gadget? gadget)
    {
        if (gadget != null)
        {
            return new GadgetDetailResponse
            {
                Id = gadget.Id,
                Seller = gadget.Seller.ToSellerResponse()!,
                Brand = gadget.Brand.ToBrandResponse()!,
                Name = gadget.Name,
                Price = gadget.Price,
                ThumbnailUrl = gadget.ThumbnailUrl,
                Status = gadget.Status,
                CreatedAt = gadget.CreatedAt,
                UpdatedAt = gadget.UpdatedAt,
                Condition = gadget.Condition,
                Quantity = gadget.Quantity,
                IsForSale = gadget.IsForSale,
                Category = gadget.Category.ToCategoryResponse()!,
                SellerStatus = gadget.Seller.User.Status,
                GadgetImages = gadget.GadgetImages.Select(g => g.ImageUrl).ToArray(),
                GadgetDescriptions = gadget.GadgetDescriptions
                                        .Select(g => new GadgetDescriptionResponse
                                        {
                                            Id = g.Id,
                                            Index = g.Index,
                                            Type = g.Type,
                                            Value = g.Value,
                                        }).ToArray(),
                SpecificationValues = gadget.SpecificationValues
                                        .Select(g => new SpecificationValueResponse
                                        {
                                            Id = g.Id,
                                            SpecificationKey = g.SpecificationKey.Name,
                                            SpecificationUnit = g.SpecificationUnit?.Name,
                                            Value = g.Value,
                                        }).ToArray(),
            };
        }
        return null;
    }
}
