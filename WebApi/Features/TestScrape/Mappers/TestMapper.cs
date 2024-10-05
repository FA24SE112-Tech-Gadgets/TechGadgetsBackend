using WebApi.Data.Entities;
using WebApi.Features.TestScrape.Models;

namespace WebApi.Features.TestScrape.Mappers;

public static class TestMapper
{
    public static GadgetResponseTest? ToGadgetResponse(this Gadget? gadget)
    {
        if (gadget != null)
        {
            return new GadgetResponseTest
            {
                Id = gadget.Id,
                Name = gadget.Name,
                Price = gadget.Price,
                ThumbnailUrl = gadget.ThumbnailUrl,
                Url = gadget.Url,
                ShopId = gadget.ShopId,
                Status = gadget.Status,
                CreatedAt = gadget.CreatedAt,
                UpdatedAt = gadget.UpdatedAt,
                Seller = gadget.Seller.ToSellerResponse(),
                Brand = gadget.Brand.ToBrandResponse(),
                Shop = gadget.Shop.ToShopResponse(),
                Category = gadget.Category.ToCategoryResponse()!,
                GadgetDescriptions = gadget.GadgetDescriptions.ToGadgetDescriptionResponse()!,
                Specifications = gadget.Specifications.ToSpecificationResponse()!,
                SpecificationKeys = gadget.SpecificationKeys.ToSpecificationKeyResponse()!,
                GadgetImages = gadget.GadgetImages.ToGadgetImageResponse()!,
            };
        }
        return null;
    }

    public static SellerResponseTest? ToSellerResponse(this Seller? seller)
    {
        if (seller != null)
        {
            return new SellerResponseTest
            {
                Id = seller.Id,
                CompanyName = seller.CompanyName,
                ShopName = seller.ShopName,
            };
        }
        return null;
    }

    public static BrandResponseTest? ToBrandResponse(this Brand? brand)
    {
        if (brand != null)
        {
            return new BrandResponseTest
            {
                Id = brand.Id,
                Name = brand.Name,
                LogoUrl = brand.LogoUrl,
            };
        }
        return null;
    }

    public static ShopResponseTest? ToShopResponse(this Shop? shop)
    {
        if (shop != null)
        {
            return new ShopResponseTest
            {
                Id = shop.Id,
                Name = shop.Name,
                WebsiteUrl = shop.WebsiteUrl,
                LogoUrl = shop.LogoUrl,
            };
        }
        return null;
    }

    public static CategoryResponseTest? ToCategoryResponse(this Category? cate)
    {
        if (cate != null)
        {
            return new CategoryResponseTest
            {
                Id = cate.Id,
                Name = cate.Name,
            };
        }
        return null;
    }

    public static ICollection<GadgetDescriptionResponseTest>? ToGadgetDescriptionResponse(this ICollection<GadgetDescription>? listGD)
    {
        if (listGD != null && listGD.Any()) // Check if the list is not null and not empty
        {
            var responseList = new List<GadgetDescriptionResponseTest>();

            foreach (var g in listGD)
            {
                if (g != null)
                {
                    responseList.Add(new GadgetDescriptionResponseTest
                    {
                        Id = g.Id,
                        GadgetId = g.GadgetId,
                        Value = g.Value,
                        Index = g.Index,
                        Type = g.Type
                    });
                }
            }

            return responseList;
        }

        return null; // Return null if the input list is null or empty
    }

    public static ICollection<GadgetResponseTest>? ToGadgetListResponse(this ICollection<Gadget>? listG)
    {
        if (listG != null && listG.Any()) // Check if the list is not null and not empty
        {
            var responseList = new List<GadgetResponseTest>();

            foreach (var gadget in listG)
            {
                if (gadget != null)
                {
                    responseList.Add(gadget.ToGadgetResponse()!);
                }
            }

            return responseList;
        }

        return null; // Return null if the input list is null or empty
    }

    public static ICollection<GadgetImageResponseTest>? ToGadgetImageResponse(this ICollection<GadgetImage>? listGI)
    {
        if (listGI != null && listGI.Any()) // Check if the list is not null and not empty
        {
            var responseList = new List<GadgetImageResponseTest>();

            foreach (var g in listGI)
            {
                if (g != null)
                {
                    responseList.Add(new GadgetImageResponseTest
                    {
                        Id = g.Id,
                        ImageUrl = g.ImageUrl,
                    });
                }
            }

            return responseList;
        }

        return null; // Return null if the input list is null or empty
    }

    public static ICollection<SpecificationValueResponseTest>? ToSpecificationValueResponse(this ICollection<SpecificationValue>? listSV)
    {
        if (listSV != null && listSV.Any()) // Check if the list is not null and not empty
        {
            var responseList = new List<SpecificationValueResponseTest>();

            foreach (var sv in listSV)
            {
                if (sv != null)
                {
                    responseList.Add(new SpecificationValueResponseTest
                    {
                        Id = sv.Id,
                        Value = sv.Value,
                    });
                }
            }

            return responseList;
        }

        return null; // Return null if the input list is null or empty
    }

    public static ICollection<SpecificationKeyResponseTest>? ToSpecificationKeyResponse(this ICollection<SpecificationKey>? listSK)
    {
        if (listSK != null && listSK.Any()) // Check if the list is not null and not empty
        {
            var responseList = new List<SpecificationKeyResponseTest>();

            foreach (var sk in listSK)
            {
                if (sk != null)
                {
                    responseList.Add(new SpecificationKeyResponseTest
                    {
                        Id = sk.Id,
                        Name = sk.Name,
                        SpecificationValues = sk.SpecificationValues.ToSpecificationValueResponse()!,
                    });
                }
            }

            return responseList;
        }

        return null; // Return null if the input list is null or empty
    }

    public static ICollection<SpecificationResponseTest>? ToSpecificationResponse(this ICollection<Specification>? listS)
    {
        if (listS != null && listS.Any()) // Check if the list is not null and not empty
        {
            var responseList = new List<SpecificationResponseTest>();

            foreach (var s in listS)
            {
                if (s != null)
                {
                    responseList.Add(new SpecificationResponseTest
                    {
                        Id = s.Id,
                        Name = s.Name,
                        SpecificationKeys = s.SpecificationKeys.ToSpecificationKeyResponse()!,
                    });
                }
            }

            return responseList;
        }

        return null; // Return null if the input list is null or empty
    }
}
