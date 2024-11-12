using WebApi.Data.Entities;
using WebApi.Features.Brands.Mappers;
using WebApi.Features.Categories.Mappers;
using WebApi.Features.Gadgets.Models;
using WebApi.Features.Users.Mappers;

namespace WebApi.Features.Gadgets.Mappers;

public static class GadgetMapper
{
    public static GadgetResponse? ToGadgetResponse(this Gadget? gadget, Guid? customerId)
    {
        if (gadget != null)
        {
            var gadgetDiscount = gadget.GadgetDiscounts.FirstOrDefault(gd => gd.Status == GadgetDiscountStatus.Active && gd.ExpiredDate > DateTime.UtcNow);
            int discountPercentage = gadget.GadgetDiscounts.Count > 0 && gadgetDiscount != null ? gadgetDiscount.DiscountPercentage : 0;
            return new GadgetResponse
            {
                Id = gadget.Id,
                Name = gadget.Name,
                Price = gadget.Price,
                DiscountPrice = (int)Math.Ceiling(gadget.Price * (1 - discountPercentage / 100.0)),
                DiscountPercentage = discountPercentage,
                DiscountExpiredDate = gadgetDiscount != null ? gadgetDiscount.ExpiredDate : null,
                SellerStatus = gadget.Seller.User.Status,
                GadgetStatus = gadget.Status,
                ThumbnailUrl = gadget.ThumbnailUrl,
                IsForSale = gadget.IsForSale,
                IsFavorite = gadget.FavoriteGadgets.Any(fg => fg.CustomerId == customerId),
            };
        }
        return null;
    }

    public static HotGadgetResponse? ToHotGadgetResponse(this Gadget? g)
    {
        if (g != null)
        {
            var gadgetDiscount = g.GadgetDiscounts.FirstOrDefault(gd => gd.Status == GadgetDiscountStatus.Active && gd.ExpiredDate > DateTime.UtcNow);
            int discountPercentage = g.GadgetDiscounts.Count > 0 && gadgetDiscount != null ? gadgetDiscount.DiscountPercentage : 0;
            return new HotGadgetResponse
            {
                Id = g.Id,
                Name = g.Name,
                SellerStatus = g.Seller.User.Status,
                Quantity = g.SellerOrderItems
                    .Where(soi => soi.SellerOrder.Status == SellerOrderStatus.Success)
                    .Sum(soi => soi.GadgetQuantity),
                ThumbnailUrl = g.ThumbnailUrl,
                Price = g.Price,
                DiscountPrice = (int)Math.Ceiling(g.Price * (1 - discountPercentage / 100.0)),
                DiscountPercentage = discountPercentage,
                DiscountExpiredDate = gadgetDiscount != null ? gadgetDiscount.ExpiredDate : null,
            };
        }
        return null;
    }

    public static GadgetRelatedToSellerResponse? ToGadgetRelatedToSellerResponse(this Gadget? gadget)
    {
        if (gadget != null)
        {
            var gadgetDiscount = gadget.GadgetDiscounts.FirstOrDefault(gd => gd.Status == GadgetDiscountStatus.Active && gd.ExpiredDate > DateTime.UtcNow);
            int discountPercentage = gadget.GadgetDiscounts.Count > 0 && gadgetDiscount != null ? gadgetDiscount.DiscountPercentage : 0;
            return new GadgetRelatedToSellerResponse
            {
                Id = gadget.Id,
                Name = gadget.Name,
                Price = gadget.Price,
                Quantity = gadget.Quantity,
                DiscountPrice = (int)Math.Ceiling(gadget.Price * (1 - discountPercentage / 100.0)),
                DiscountPercentage = discountPercentage,
                DiscountExpiredDate = gadgetDiscount != null ? gadgetDiscount.ExpiredDate : null,
                ThumbnailUrl = gadget.ThumbnailUrl,
                IsForSale = gadget.IsForSale,
                GadgetStatus = gadget.Status,
            };
        }
        return null;
    }

    public static GadgetDetailResponse? ToGadgetDetailResponse(this Gadget? gadget, Guid? customerId)
    {
        if (gadget != null)
        {
            var gadgetDiscount = gadget.GadgetDiscounts.FirstOrDefault(gd => gd.Status == GadgetDiscountStatus.Active && gd.ExpiredDate > DateTime.UtcNow);
            int discountPercentage = gadget.GadgetDiscounts.Count > 0 && gadgetDiscount != null ? gadgetDiscount.DiscountPercentage : 0;
            return new GadgetDetailResponse
            {
                Id = gadget.Id,
                Seller = gadget.Seller.ToSellerResponse()!,
                Brand = gadget.Brand.ToBrandResponse()!,
                Name = gadget.Name,
                Price = gadget.Price,
                DiscountPrice = (int)Math.Ceiling(gadget.Price * (1 - discountPercentage / 100.0)),
                DiscountPercentage = discountPercentage,
                DiscountExpiredDate = gadgetDiscount != null ? gadgetDiscount.ExpiredDate : null,
                ThumbnailUrl = gadget.ThumbnailUrl,
                Status = gadget.Status,
                CreatedAt = gadget.CreatedAt,
                UpdatedAt = gadget.UpdatedAt,
                Condition = gadget.Condition,
                Quantity = gadget.Quantity,
                IsForSale = gadget.IsForSale,
                IsFavorite = gadget.FavoriteGadgets.Any(fg => fg.CustomerId == customerId),
                Category = gadget.Category.ToCategoryResponse()!,
                SellerStatus = gadget.Seller.User.Status,
                GadgetImages = gadget.GadgetImages
                                        .Select(g => new GadgetImageResponse
                                        {
                                            Id = g.Id,
                                            ImageUrl = g.ImageUrl
                                        }).ToArray(),
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

    public static List<GadgetResponse>? ToListGadgetsResponse(this List<Gadget> gadgets, Guid? customerId)
    {
        if (gadgets != null)
        {
            return gadgets
            .Select(gadget => gadget.ToGadgetResponse(customerId))
            .ToList()!;
        }
        return null;
    }
}
