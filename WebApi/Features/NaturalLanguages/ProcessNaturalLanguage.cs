using FluentValidation;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;
using WebApi.Common.Filters;
using WebApi.Common.Utils;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Gadgets.Mappers;
using WebApi.Features.NaturalLanguages.Models;
using WebApi.Features.Sellers.Mappers;
using WebApi.Services.AI;
using WebApi.Services.Auth;
using WebApi.Services.Embedding;

namespace WebApi.Features.NaturalLanguages;

[ApiController]
[RequestValidation<Request>]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class ProcessNaturalLanguage : ControllerBase
{
    public new class Request
    {
        public string Input { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Input)
                .NotEmpty().WithMessage("Input không được để trống");
        }
    }

    [Tags("Natural Languages")]
    [HttpPost("natural-languages/search")]
    [SwaggerOperation(Summary = "Search With Natural Language",
        Description = """
        This API is for searching with natural language
        """
    )]
    public async Task<IActionResult> Handler(Request request, NaturalLanguageService naturalLanguageService, EmbeddingService embeddingService, AppDbContext context,
        CurrentUserService currentUserService)
    {
        var user = await currentUserService.GetCurrentUser();
        var customerId = user?.Customer?.Id;

        var query = await naturalLanguageService.GetRequestByUserInput(request.Input);
        if (query == null)
        {
            return StatusCode(500, "Lỗi khi xử lí lệnh ngôn ngữ tự nhiên.");
        }

        DateTime currentDate = DateTime.UtcNow;

        Expression<Func<Gadget, double>> effectivePrice = g =>
                g.Price - g.GadgetDiscounts
                .Where(d => d.Status == GadgetDiscountStatus.Active && d.ExpiredDate > currentDate)
                .Sum(d => g.Price / 100.0 * d.DiscountPercentage);

        if (query.IsSearchingSeller)
        {
            var pricePredicate = PredicateBuilder.New<Seller>(true);
            pricePredicate = pricePredicate.Or(s =>
                s.Gadgets.Any(g =>
                     effectivePrice.Invoke(g) >= query.MinPrice
                     && effectivePrice.Invoke(g) <= query.MaxPrice
                )
            );

            var categoryPredicate = PredicateBuilder.New<Seller>(true);
            foreach (var category in query.Categories)
            {
                categoryPredicate = categoryPredicate.Or(s => s.Gadgets.Any(g => g.Category.Name.ToLower() == category.ToLower()));
            }

            var brandPredicate = PredicateBuilder.New<Seller>(true);
            foreach (var brand in query.Brands)
            {
                brandPredicate = brandPredicate.Or(s => s.Gadgets.Any(g => g.Brand.Name.ToLower() == brand.ToLower()));
            }

            var purposePredicate = PredicateBuilder.New<Seller>(true);
            foreach (var purpose in query.Purposes)
            {
                if (new List<string> { "Học tập" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(s => s.Gadgets.Any(g =>
                                                g.Name.ToLower().Contains("học tập")
                                                || g.Name.ToLower().Contains("sinh viên")
                                                || g.Name.ToLower().Contains("học sinh")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("học tập"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("sinh viên"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("học sinh"))));
                }

                if (new List<string> { "Làm việc", "Văn phòng" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(s => s.Gadgets.Any(
                                                g => g.Name.ToLower().Contains("văn phòng")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("văn phòng"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("công sở"))));
                }

                if (new List<string> { "Gaming", "Giải trí" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(s => s.Gadgets.Any(
                                                g => g.Name.ToLower().Contains("gaming")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("gaming"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("giải trí"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("chơi game"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("liên quân"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("liên minh"))));
                }

                if (new List<string> { "Vận động", "Ngoài trời", "Thể thao" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(s => s.Gadgets.Any(
                                                g => g.Name.Contains("sport")
                                                || g.Name.ToLower().Contains("chạy bộ")
                                                || g.Name.ToLower().Contains("thể dục")
                                                || g.Name.ToLower().Contains("thể thao")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("vận động"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("ngoài trời"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("đi dạo"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("chạy bộ"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("sport"))));
                }

                if (new List<string> { "Thiết kế đồ họa", "Designer", "IT" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(s => s.Gadgets.Any(
                                                g => g.Name.Contains("đồ hoạ")
                                                || g.Name.ToLower().Contains("it")
                                                || g.Name.ToLower().Contains("gaming")
                                                || g.Name.ToLower().Contains("giải trí")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("đồ hoạ"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("cấu hình cao"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("thiết kế"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("công nghệ thông tin"))));
                }

                if (new List<string> { "Bơi lội" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(s => s.Gadgets.Any(
                                                g => g.Name.Contains("nước")
                                                || g.Name.ToLower().Contains("bơi")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("chống nước"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("kháng nước"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("bơi"))));
                }
            }

            var segmentationPredicate = PredicateBuilder.New<Seller>(true);
            foreach (var segmentation in query.Segmentations)
            {
                if (new List<string> { "Giá rẻ", "Giá tốt", "Giá sinh viên" }.Contains(segmentation))
                {
                    segmentationPredicate = segmentationPredicate.Or(s => s.Gadgets.Any(g =>
                            g.Category.Name == "Điện thoại" && effectivePrice.Invoke(g) <= 4_000_000
                            || g.Category.Name == "Laptop" && effectivePrice.Invoke(g) <= 10_000_000
                            || g.Category.Name == "Tai nghe" && effectivePrice.Invoke(g) <= 500_000
                            || g.Category.Name == "Loa" && effectivePrice.Invoke(g) <= 2_000_000
                        ));
                }

                if (new List<string> { "Tầm trung" }.Contains(segmentation))
                {
                    segmentationPredicate = segmentationPredicate.Or(s => s.Gadgets.Any(g =>
                            g.Category.Name == "Điện thoại" && 4_000_000 < effectivePrice.Invoke(g) && effectivePrice.Invoke(g) <= 13_000_000
                            || g.Category.Name == "Laptop" && 10_000_000 < effectivePrice.Invoke(g) && effectivePrice.Invoke(g) <= 25_000_000
                            || g.Category.Name == "Tai nghe" && 500_000 < effectivePrice.Invoke(g) && effectivePrice.Invoke(g) <= 2_000_000
                            || g.Category.Name == "Loa" && 2_000_000 < effectivePrice.Invoke(g) && effectivePrice.Invoke(g) <= 7_000_000
                        ));
                }

                if (new List<string> { "Cao cấp", "Hiện đại" }.Contains(segmentation))
                {
                    segmentationPredicate = segmentationPredicate.Or(s => s.Gadgets.Any(g =>
                            g.Category.Name == "Điện thoại" && effectivePrice.Invoke(g) > 13_000_000
                            || g.Category.Name == "Laptop" && effectivePrice.Invoke(g) > 25_000_000
                            || g.Category.Name == "Tai nghe" && effectivePrice.Invoke(g) > 2_000_000
                            || g.Category.Name == "Loa" && effectivePrice.Invoke(g) > 7_000_000
                        ));
                }
            }

            var locationPredicate = PredicateBuilder.New<Seller>(true);
            if (query.Locations.Any())
            {
                //var locationVectors = await embeddingService.GetEmbeddings(query.Locations);

                //locationPredicate = locationPredicate.Or(s =>
                //    locationVectors.Any(vector => 1 - s.AddressVector.CosineDistance(vector) >= 0.5)
                //);

                locationPredicate = locationPredicate.Or(g =>
                    query.Locations.Any(l => g.ShopAddress.ToLower().Contains(l.ToLower()))
                );
            }

            var originPredicate = PredicateBuilder.New<Seller>(true);
            if (query.Origins.Any())
            {
                //var originVectors = await embeddingService.GetEmbeddings(query.Origins);

                //originPredicate = originPredicate.Or(s => s.Gadgets.Any(g =>
                //    g.SpecificationValues.Any(sv =>
                //        sv.SpecificationKey.Name == "Xuất xứ"
                //        && originVectors.Any(vector => 1 - sv.Vector.CosineDistance(vector) >= 0.6)
                //    )
                //));

                originPredicate = originPredicate.Or(s => s.Gadgets.Any(g =>
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Xuất xứ"
                        && query.Origins.Any(o => sv.Value.ToLower().Contains(o.ToLower()))
                    )
                ));
            }

            var highRatingPredicate = PredicateBuilder.New<Seller>(true);
            if (query.IsHighRating)
            {
                highRatingPredicate = highRatingPredicate.Or(s =>
                    s.SellerOrders
                        .SelectMany(order => order.SellerOrderItems)
                        .Where(item => item.Review != null)
                        .Average(item => item.Review!.Rating) >= 4
                );
            }

            var positiveReviewPredicate = PredicateBuilder.New<Seller>(true);
            if (query.IsPositiveReview)
            {
                positiveReviewPredicate = positiveReviewPredicate.Or(s =>
                        s.SellerOrders
                        .SelectMany(order => order.SellerOrderItems)
                        .Where(item => item.Review != null)
                        .Count(item => item.Review!.IsPositive == true) >
                        s.SellerOrders
                        .SelectMany(order => order.SellerOrderItems)
                        .Where(item => item.Review != null)
                        .Count(item => item.Review!.IsPositive == false)
                );
            }

            var discountedPredicate = PredicateBuilder.New<Seller>(true);
            if (query.IsDiscounted)
            {
                discountedPredicate = discountedPredicate.Or(s => s.Gadgets.Any(g =>
                    g.GadgetDiscounts.Any(gd => gd.Status == GadgetDiscountStatus.Active && gd.ExpiredDate > currentDate)
                ));
            }

            var outerPredicate = PredicateBuilder.New<Seller>(true);
            outerPredicate = outerPredicate
                                    .And(pricePredicate)
                                    .And(categoryPredicate)
                                    .And(brandPredicate)
                                    .And(purposePredicate)
                                    .And(segmentationPredicate)
                                    .And(locationPredicate)
                                    .And(originPredicate)
                                    .And(highRatingPredicate)
                                    .And(positiveReviewPredicate)
                                    .And(discountedPredicate)
                                    ;

            var sellers = await context.Sellers.AsExpandable()
                                 .Where(outerPredicate)
                                 .Where(s => s.User.Status == UserStatus.Active)
                                 .OrderByDescending(s => query.IsBestSeller
                                            ? s.SellerOrders
                                                .Where(so => so.Status == SellerOrderStatus.Success)
                                                .SelectMany(so => so.SellerOrderItems)
                                                .Sum(soi => soi.GadgetQuantity)
                                            : 0)
                                 .Select(s => s.ToSellerDetailResponse()!)
                                 .Take(30)
                                 .ToListAsync();

            return Ok(new ProcessNaturalLanguageResponse
            {
                Count = sellers.Count,
                Sellers = sellers,
                Type = "seller"
            });
        }
        else
        {
            var pricePredicate = PredicateBuilder.New<Gadget>(true);
            pricePredicate = pricePredicate.Or(g =>
                effectivePrice.Invoke(g) >= query.MinPrice &&
                effectivePrice.Invoke(g) <= query.MaxPrice);

            var categoryPredicate = PredicateBuilder.New<Gadget>(true);
            foreach (var category in query.Categories)
            {
                categoryPredicate = categoryPredicate.Or(g => g.Category.Name.ToLower() == category.ToLower());
            }

            var brandPredicate = PredicateBuilder.New<Gadget>(true);
            foreach (var brand in query.Brands)
            {
                brandPredicate = brandPredicate.Or(g => g.Brand.Name.ToLower() == brand.ToLower());
            }

            var purposePredicate = PredicateBuilder.New<Gadget>(true);
            foreach (var purpose in query.Purposes)
            {
                if (new List<string> { "Học tập" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(g => g.Name.ToLower().Contains("học tập")
                                                || g.Name.ToLower().Contains("sinh viên")
                                                || g.Name.ToLower().Contains("học sinh")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("học tập"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("sinh viên"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("học sinh")));
                }

                if (new List<string> { "Làm việc", "Văn phòng" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(g => g.Name.ToLower().Contains("văn phòng")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("văn phòng"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("công sở")));
                }

                if (new List<string> { "Gaming", "Giải trí" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(g => g.Name.ToLower().Contains("gaming")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("gaming"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("giải trí"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("chơi game"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("liên quân"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("liên minh")));
                }

                if (new List<string> { "Vận động", "Ngoài trời", "Thể thao" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(g => g.Name.Contains("sport")
                                                || g.Name.ToLower().Contains("chạy bộ")
                                                || g.Name.ToLower().Contains("thể dục")
                                                || g.Name.ToLower().Contains("thể thao")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("vận động"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("ngoài trời"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("đi dạo"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("chạy bộ"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("sport")));
                }

                if (new List<string> { "Thiết kế đồ họa", "Designer", "IT" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(g => g.Name.Contains("đồ hoạ")
                                                || g.Name.ToLower().Contains("it")
                                                || g.Name.ToLower().Contains("gaming")
                                                || g.Name.ToLower().Contains("giải trí")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("đồ hoạ"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("cấu hình cao"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("thiết kế"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("công nghệ thông tin")));
                }

                if (new List<string> { "Bơi lội" }.Contains(purpose))
                {
                    purposePredicate = purposePredicate.And(g => g.Name.Contains("nước")
                                                || g.Name.ToLower().Contains("bơi")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("chống nước"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("kháng nước"))
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("bơi")));
                }
            }

            var goodBatteryLifePredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsGoodBatteryLife)
            {
                goodBatteryLifePredicate = goodBatteryLifePredicate.Or(g =>
                                g.SpecificationValues.Any(sv => sv.SpecificationKey.Name == "Dung lượng pin" && Convert.ToDouble(sv.Value) >= 5000));
            }

            var fastChargePredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsFastCharge)
            {
                var fastChargeVector = await embeddingService.GetEmbedding("sạc nhanh");

                fastChargePredicate = fastChargePredicate.Or(g => g.SpecificationValues.Any(sv => sv.SpecificationKey.Name == "Tính năng" &&
                                        1 - sv.Vector.CosineDistance(fastChargeVector) > 0.7));
            }

            var wideScreenPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsWideScreen)
            {
                wideScreenPredicate = wideScreenPredicate.Or(g =>
                            g.SpecificationValues
                                 .Any(sv => sv.SpecificationKey.Name == "Màn hình" && Convert.ToDouble(sv.Value) >= 16));
            }

            var smallScreenPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsSmallScreen)
            {
                smallScreenPredicate = smallScreenPredicate.Or(g =>
                            g.SpecificationValues
                                 .Any(sv => sv.SpecificationKey.Name == "Màn hình" && Convert.ToDouble(sv.Value) < 16));
            }

            var foldablePredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsFoldable)
            {
                foldablePredicate = foldablePredicate.Or(g =>
                        g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("gập")
                                                        || desc.Value.ToLower().Contains("công nghệ gập")
                                                        || desc.Value.ToLower().Contains("thiết kế gập")
                                                        || desc.Value.ToLower().Contains("màn hình gập")
                                                        || desc.Value.ToLower().Contains("điện thoại gập")));
            }

            var inchPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Categories.Contains("Điện thoại"))
            {
                inchPredicate = inchPredicate.And(g =>
                    g.Category.Name != "Điện thoại" ||
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Màn hình rộng"
                        && Convert.ToDouble(sv.Value) >= query.MinInch
                        && Convert.ToDouble(sv.Value) <= query.MaxInch
                        ));
            }
            if (query.Categories.Contains("Laptop"))
            {
                inchPredicate = inchPredicate.And(g =>
                    g.Category.Name != "Laptop" ||
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Màn hình"
                        && Convert.ToDouble(sv.Value) >= query.MinInch
                        && Convert.ToDouble(sv.Value) <= query.MaxInch
                        ));
            }

            var highResolutionPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsHighResolution
                //&& query.Categories.Contains("Điện thoại")
                )
            {
                var phoneHighResolutionVectors = await embeddingService.GetEmbeddings(["Full HD", "Retina", "2K", "1.5K"]);

                highResolutionPredicate = highResolutionPredicate.Or(g =>
                    g.Category.Name == "Điện thoại" &&
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Độ phân giải màn hình"
                        && phoneHighResolutionVectors.Any(vector =>
                                1 - sv.Vector.CosineDistance(vector) >= 0.5)
                        ));
            }
            if (query.IsHighResolution
                //&& query.Categories.Contains("Laptop")
                )
            {
                var laptopHighResolutionVectors = await embeddingService.GetEmbeddings(["Full HD", "QHD", "2.8K", "WUXGA", "2.5K", "4K", "3.2K", "Retina"]);

                highResolutionPredicate = highResolutionPredicate.Or(g =>
                    g.Category.Name == "Laptop" &&
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Độ phân giải"
                        && laptopHighResolutionVectors.Any(vector => 1 - sv.Vector.CosineDistance(vector) >= 0.5)
                        ));
            }

            var operatingSystemPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.OperatingSystems.Any())
            {
                //var operatingSystemVectors = await embeddingService.GetEmbeddings(query.OperatingSystems);
                //operatingSystemPredicate = operatingSystemPredicate.Or(g =>
                //    g.Category.Name == "Laptop" &&
                //    g.SpecificationValues.Any(sv =>
                //        sv.SpecificationKey.Name == "Hệ điều hành"
                //        && operatingSystemVectors.Any(vector => 1 - sv.Vector.CosineDistance(vector) >= 0.6)
                //));

                //operatingSystemPredicate = operatingSystemPredicate.Or(g =>
                //    g.Category.Name == "Điện thoại" &&
                //    g.SpecificationValues.Any(sv =>
                //        sv.SpecificationKey.Name == "Hệ điều hành"
                //        && operatingSystemVectors.Any(vector => 1 - sv.Vector.CosineDistance(vector) >= 0.6)
                //));

                operatingSystemPredicate = operatingSystemPredicate.Or(g =>
                    g.Category.Name == "Laptop" &&
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Hệ điều hành"
                        && query.OperatingSystems.Any(os => sv.Value.ToLower().Contains(os.ToLower()) || os.ToLower().Contains(sv.Value.ToLower()))
                ));

                operatingSystemPredicate = operatingSystemPredicate.Or(g =>
                    g.Category.Name == "Điện thoại" &&
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Hệ điều hành"
                        && query.OperatingSystems.Any(os => sv.Value.ToLower().Contains(os.ToLower()) || os.ToLower().Contains(sv.Value.ToLower()))
                ));
            }

            var storageCapacityPhonePredicate = PredicateBuilder.New<Gadget>(true);
            if (query.StorageCapacitiesPhone.Any())
            {
                var numbers = query.StorageCapacitiesPhone.Select(x => x.ExtractNumberAndString().number.ToString()).ToList();
                var units = query.StorageCapacitiesPhone.Select(x => x.ExtractNumberAndString().text).ToList();

                storageCapacityPhonePredicate = storageCapacityPhonePredicate.Or(g =>
                    g.Category.Name != "Điện thoại" ||
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Dung lượng lưu trữ"
                        && numbers.Contains(sv.Value)
                        && units.Contains(sv.SpecificationUnit.Name)
                ));
            }

            var storageCapacityLaptopPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.StorageCapacitiesLaptop.Any())
            {
                var numbers = query.StorageCapacitiesLaptop.Select(x => x.ExtractNumberAndString().number.ToString()).ToList();
                var units = query.StorageCapacitiesLaptop.Select(x => x.ExtractNumberAndString().text).ToList();

                storageCapacityLaptopPredicate = storageCapacityLaptopPredicate.Or(g =>
                    g.Category.Name != "Laptop" ||
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Ổ cứng SSD"
                        && numbers.Contains(sv.Value)
                        && units.Contains(sv.SpecificationUnit.Name)
                ));
            }

            var ramPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Rams.Any())
            {
                var numbers = query.Rams.Select(x => x.ExtractNumberAndString().number.ToString()).ToList();
                var units = query.Rams.Select(x => x.ExtractNumberAndString().text).ToList();

                ramPredicate = ramPredicate.Or(g =>
                    g.Category.Name == "Laptop" &&
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "RAM"
                        && numbers.Contains(sv.Value)
                        && units.Contains(sv.SpecificationUnit.Name)
                ));

                ramPredicate = ramPredicate.Or(g =>
                    g.Category.Name == "Điện thoại" &&
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "RAM"
                        && numbers.Contains(sv.Value)
                        && units.Contains(sv.SpecificationUnit.Name)
                ));
            }

            var featurePredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Features.Any())
            {
                var featureVectors = await embeddingService.GetEmbeddings(query.Features);

                featurePredicate = featurePredicate.Or(g =>
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Tính năng"
                        && featureVectors.Any(vector => 1 - sv.Vector.CosineDistance(vector) >= 0.58)
                    )
                );
            }

            var conditionPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Conditions.Any())
            {
                var conditionVectors = await embeddingService.GetEmbeddings(query.Conditions);

                featurePredicate = featurePredicate.Or(g =>
                    conditionVectors.Any(vector => 1 - g.ConditionVector.CosineDistance(vector) >= 0.5)
                );
            }

            var segmentationPredicate = PredicateBuilder.New<Gadget>(true);
            foreach (var segmentation in query.Segmentations)
            {
                if (new List<string> { "Giá rẻ", "Giá tốt", "Giá sinh viên" }.Contains(segmentation))
                {
                    segmentationPredicate = segmentationPredicate.Or(g =>
                            g.Category.Name == "Điện thoại" && effectivePrice.Invoke(g) <= 4_000_000
                            || g.Category.Name == "Laptop" && effectivePrice.Invoke(g) <= 10_000_000
                            || g.Category.Name == "Tai nghe" && effectivePrice.Invoke(g) <= 500_000
                            || g.Category.Name == "Loa" && effectivePrice.Invoke(g) <= 2_000_000
                        );
                }

                if (new List<string> { "Tầm trung" }.Contains(segmentation))
                {
                    segmentationPredicate = segmentationPredicate.Or(g =>
                            g.Category.Name == "Điện thoại" && 4_000_000 < effectivePrice.Invoke(g) && effectivePrice.Invoke(g) <= 13_000_000
                            || g.Category.Name == "Laptop" && 10_000_000 < effectivePrice.Invoke(g) && effectivePrice.Invoke(g) <= 25_000_000
                            || g.Category.Name == "Tai nghe" && 500_000 < effectivePrice.Invoke(g) && effectivePrice.Invoke(g) <= 2_000_000
                            || g.Category.Name == "Loa" && 2_000_000 < effectivePrice.Invoke(g) && effectivePrice.Invoke(g) <= 7_000_000
                        );
                }

                if (new List<string> { "Cao cấp", "Hiện đại" }.Contains(segmentation))
                {
                    segmentationPredicate = segmentationPredicate.Or(g =>
                            g.Category.Name == "Điện thoại" && effectivePrice.Invoke(g) > 13_000_000
                            || g.Category.Name == "Laptop" && effectivePrice.Invoke(g) > 25_000_000
                            || g.Category.Name == "Tai nghe" && effectivePrice.Invoke(g) > 2_000_000
                            || g.Category.Name == "Loa" && effectivePrice.Invoke(g) > 7_000_000
                        );
                }
            }

            var locationPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Locations.Count != 0)
            {
                //var locationVectors = await embeddingService.GetEmbeddings(query.Locations);

                //locationPredicate = locationPredicate.Or(g =>
                //    locationVectors.Any(vector => 1 - g.Seller.AddressVector.CosineDistance(vector) >= 0.5)
                //);

                locationPredicate = locationPredicate.Or(g =>
                    query.Locations.Any(l => g.Seller.ShopAddress.ToLower().Contains(l.ToLower()) || l.ToLower().Contains(g.Seller.ShopAddress.ToLower()))
                );
            }

            var originPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Origins.Count != 0)
            {
                //var originVectors = await embeddingService.GetEmbeddings(query.Origins);

                //originPredicate = originPredicate.Or(g =>
                //    g.SpecificationValues.Any(sv =>
                //        sv.SpecificationKey.Name == "Xuất xứ"
                //        && originVectors.Any(vector => 1 - sv.Vector.CosineDistance(vector) >= 0.6)
                //    )
                //);

                originPredicate = originPredicate.Or(g =>
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Xuất xứ"
                        && query.Origins.Any(o => sv.Value.ToLower().Contains(o.ToLower()) || o.ToLower().Contains(sv.Value.ToLower()))
                    )
                );
            }

            var releaseDatePredicate = PredicateBuilder.New<Gadget>(true);
            if (query.ReleaseDate.Count != 0)
            {
                releaseDatePredicate = releaseDatePredicate.Or(g =>
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Thời điểm ra mắt"
                        && query.ReleaseDate.Any(r => sv.Value.Contains(r) || r.Contains(sv.Value))
                    )
                );
            }

            var colorPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Colors.Any())
            {
                //var colorVectors = await embeddingService.GetEmbeddings(query.Colors);

                //colorPredicate = colorPredicate.Or(g =>
                //    g.SpecificationValues.Any(sv =>
                //        sv.SpecificationKey.Name == "Màu sắc"
                //        && colorVectors.Any(vector => 1 - sv.Vector.CosineDistance(vector) >= 0.6)
                //    )
                //);

                colorPredicate = colorPredicate.Or(g =>
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Màu sắc"
                        && query.Colors.Any(c => sv.Value.ToLower().Contains(c.ToLower()))
                    )
                );

                colorPredicate = colorPredicate.Or(g
                    => g.GadgetDescriptions.Any(gd
                        => query.Colors.Any(c => gd.Value.Substring(gd.Value.ToLower().IndexOf("màu "), 50).Contains(c.ToLower()))
                    )
                );
            }

            var AIPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsAi)
            {
                AIPredicate = AIPredicate.Or(g =>
                    g.GadgetDescriptions.Any(desc => desc.Value.Contains(" AI ")) ||
                    g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("trí tuệ nhân tạo"))
                );
            }

            var energySavingPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsEnergySaving)
            {
                energySavingPredicate = energySavingPredicate.Or(g =>
                    g.Name.ToLower().Contains("tiết kiệm điện") ||
                    g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("tiêu thụ điện thấp")) ||
                    g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("điện năng thấp")) ||
                    g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("tiết kiệm điện")) ||
                    g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("ít điện"))
                );
            }

            var discountedPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsDiscounted)
            {
                discountedPredicate = discountedPredicate.Or(g =>
                    g.GadgetDiscounts.Any(gd => gd.Status == GadgetDiscountStatus.Active && gd.ExpiredDate > currentDate)
                );
            }

            var highRatingPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsHighRating)
            {
                highRatingPredicate = highRatingPredicate.Or(g =>
                    g.SellerOrderItems.Where(soi => soi.Review != null)
                                      .Average(soi => soi.Review!.Rating) >= 4
                );
            }

            var positiveReviewPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsPositiveReview)
            {
                positiveReviewPredicate = positiveReviewPredicate.Or(g =>
                   g.SellerOrderItems.Count(so => so.Review!.IsPositive == true) >
                   g.SellerOrderItems.Count(so => so.Review!.IsPositive == false)
                );
            }

            var availablePredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsAvailable)
            {
                availablePredicate = availablePredicate.Or(g =>
                    g.Quantity > 0 && g.IsForSale == true
                );
            }

            var categoryTypePredicate = PredicateBuilder.New<Gadget>(true);
            foreach (var categoryType in query.CategoryTypes)
            {
                if (new List<string> { "Tai nghe Bluetooth" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Tai nghe" &&
                                                (g.Name.ToLower().Contains("bluetooth")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("bluetooth"))
                                                )
                                             );
                }

                if (new List<string> { "Tai nghe có dây" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Tai nghe" &&
                                                (g.Name.ToLower().Contains("có dây")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("có dây"))
                                                )
                                             );
                }

                if (new List<string> { "Tai nghe chụp tai" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Tai nghe" &&
                                                (g.Name.ToLower().Contains("chụp tai")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("chụp tai"))
                                                )
                                             );
                }

                if (new List<string> { "Tai nghe gaming" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Tai nghe" &&
                                                (g.Name.ToLower().Contains("gaming")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("gaming"))
                                                )
                                             );
                }

                if (new List<string> { "Tai nghe gaming" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Tai nghe" &&
                                                (g.Name.ToLower().Contains("gaming")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("gaming"))
                                                )
                                             );
                }

                if (new List<string> { "Loa Bluetooth" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Loa" &&
                                                (g.Name.ToLower().Contains("bluetooth")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("bluetooth"))
                                                )
                                             );
                }

                if (new List<string> { "Loa kéo" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Loa" &&
                                                (g.Name.ToLower().Contains("kéo")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("kéo"))
                                                )
                                             );
                }

                if (new List<string> { "Loa karaoke" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Loa" &&
                                                (g.Name.ToLower().Contains("karaoke")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("karaoke"))
                                                )
                                             );
                }

                if (new List<string> { "Loa điện" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Loa" &&
                                                (g.Name.ToLower().Contains("loa điện")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("loa điện"))
                                                )
                                             );
                }

                if (new List<string> { "Loa vi tính" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Loa" &&
                                                (g.Name.ToLower().Contains("loa vi tính")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("loa vi tính"))
                                                )
                                             );
                }

                if (new List<string> { "Loa thanh" }.Contains(categoryType))
                {
                    categoryTypePredicate = categoryTypePredicate.And(g => g.Category.Name == "Loa" &&
                                                (g.Name.ToLower().Contains("loa thanh")
                                                || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("loa thanh"))
                                                )
                                             );
                }

            }

            var outerPredicate = PredicateBuilder.New<Gadget>(true);
            outerPredicate = outerPredicate
                                .And(pricePredicate)
                                .And(categoryPredicate)
                                .And(brandPredicate)
                                .And(purposePredicate)
                                .And(goodBatteryLifePredicate)
                                .And(fastChargePredicate)
                                .And(wideScreenPredicate)
                                .And(smallScreenPredicate)
                                .And(foldablePredicate)
                                .And(inchPredicate)
                                .And(highResolutionPredicate)
                                .And(operatingSystemPredicate)
                                .And(storageCapacityPhonePredicate)
                                .And(storageCapacityLaptopPredicate)
                                .And(ramPredicate)
                                .And(featurePredicate)
                                .And(segmentationPredicate)
                                .And(locationPredicate)
                                .And(originPredicate)
                                .And(releaseDatePredicate)
                                .And(colorPredicate)
                                .And(AIPredicate)
                                .And(energySavingPredicate)
                                .And(discountedPredicate)
                                .And(highRatingPredicate)
                                .And(positiveReviewPredicate)
                                .And(availablePredicate)
                                .And(categoryTypePredicate);

            var input = request.Input.Length > 512 ? request.Input[0..512] : request.Input;
            var inputVector = await embeddingService.GetEmbeddingOpenAI(input);

            var gadgets = await context.Gadgets.AsExpandable()
                                        .Include(c => c.Seller)
                                            .ThenInclude(s => s.User)
                                        .Include(c => c.FavoriteGadgets)
                                        .Include(g => g.GadgetDiscounts)
                                        .Where(outerPredicate)
                                        .Where(g => g.Status == GadgetStatus.Active && g.Seller.User.Status == UserStatus.Active)
                                        .OrderByDescending(g => query.IsBestGadget
                                            ? g.SellerOrderItems
                                                .Where(soi => soi.SellerOrder.Status == SellerOrderStatus.Success)
                                                .Sum(soi => soi.GadgetQuantity)
                                            : 1 - g.Vector!.CosineDistance(inputVector))
                                        .Select(g => g.ToGadgetResponse(customerId)!)
                                        .Take(30)
                                        .ToListAsync();


            return Ok(new ProcessNaturalLanguageResponse
            {
                Count = gadgets.Count,
                Gadgets = gadgets,
                Type = "gadget"
            });
        }
    }
}
