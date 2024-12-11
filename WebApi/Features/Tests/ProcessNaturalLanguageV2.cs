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
using WebApi.Services.Auth;
using WebApi.Services.Embedding;
using WebApi.Services.NaturalLanguage;

namespace WebApi.Features.Tests;

[ApiController]
[RequestValidation<Request>]
public class ProcessNaturalLanguageV2 : ControllerBase
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

    [Tags("Tests")]
    [HttpPost("tests/natural-languages-v2/search")]
    [SwaggerOperation(Summary = "Search With Natural Language",
        Description = """
        This API is for searching with natural language
        """
    )]
    public async Task<IActionResult> Handler(Request request, NaturalLanguageServiceV2 naturalLanguageService, EmbeddingService embeddingService, AppDbContext context,
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
            var keywordGroupPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Keywords.Count > 0)
            {
                var groups = await context.NaturalLanguageKeywordGroups
                                            .Include(g => g.Criteria)
                                                .ThenInclude(c => c.SpecificationKey)
                                            .Include(g => g.Criteria)
                                                .ThenInclude(c => c.Categories)
                                            .Where(g => query.Keywords.Any(k => g.NaturalLanguageKeywords.Any(kk => k.ToLower() == kk.Keyword.ToLower())))
                                            .Where(g => g.Status == NaturalLanguageKeywordGroupStatus.Active)
                                            .ToListAsync();

                //var groups = await context.NaturalLanguageKeywordGroups
                //                            .Include(g => g.Criteria)
                //                                .ThenInclude(c => c.SpecificationKey)
                //                            .Include(g => g.Criteria)
                //                                .ThenInclude(c => c.Categories)
                //                            .Where(g => query.KeywordGroups.Any(kg => kg.ToLower() == g.Name.ToLower()))
                //                            .Where(g => g.Status == NaturalLanguageKeywordGroupStatus.Active)
                //                            .ToListAsync();

                foreach (var group in groups)
                {
                    var groupPredicate = PredicateBuilder.New<Gadget>(true);

                    foreach (var criteria in group.Criteria)
                    {
                        var criteriaPredicate = PredicateBuilder.New<Gadget>(true);
                        var categoryIds = criteria.CriteriaCategories.Select(c => c.CategoryId).ToList();

                        if (criteria.Type == CriteriaType.Name)
                        {
                            criteriaPredicate = criteriaPredicate.Start(g => g.Name.ToLower().Contains(criteria.Contains!.ToLower()));
                            criteriaPredicate = criteriaPredicate.And(g => categoryIds.Contains(g.CategoryId));
                        }

                        if (criteria.Type == CriteriaType.Description)
                        {
                            criteriaPredicate = criteriaPredicate.Start(g => g.GadgetDescriptions.Any(gd => gd.Value.ToLower().Contains(criteria.Contains!.ToLower())));
                            criteriaPredicate = criteriaPredicate.And(g => categoryIds.Contains(g.CategoryId));
                        }

                        if (criteria.Type == CriteriaType.Condition)
                        {
                            criteriaPredicate = criteriaPredicate.Start(g => g.Condition.ToLower().Contains(criteria.Contains!.ToLower()));
                            criteriaPredicate = criteriaPredicate.And(g => categoryIds.Contains(g.CategoryId));
                        }

                        if (criteria.Type == CriteriaType.Price)
                        {

                            criteriaPredicate = criteriaPredicate.Start(g =>
                                effectivePrice.Invoke(g) >= criteria.MinPrice && effectivePrice.Invoke(g) <= criteria.MaxPrice);

                            criteriaPredicate = criteriaPredicate.And(g => categoryIds.Contains(g.CategoryId));
                        }

                        if (criteria.Type == CriteriaType.Specification)
                        {
                            criteriaPredicate = criteriaPredicate.Start(g => g.SpecificationValues
                                                    .Any(sv => sv.SpecificationKeyId == criteria.SpecificationKeyId && sv.Value.ToLower().Contains(criteria.Contains!.ToLower())));
                        }

                        groupPredicate = groupPredicate.Or(criteriaPredicate);
                    }

                    keywordGroupPredicate = keywordGroupPredicate.And(groupPredicate);
                }
            }

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

            var operatingSystemPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.OperatingSystems.Any())
            {
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
                    g.Category.Name == "Điện thoại" &&
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
                    g.Category.Name == "Laptop" &&
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name.StartsWith("Ổ cứng")
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

            var locationPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Locations.Count != 0)
            {
                locationPredicate = locationPredicate.Or(g =>
                    query.Locations.Any(l => g.Seller.ShopAddress.ToLower().Contains(l.ToLower()) || l.ToLower().Contains(g.Seller.ShopAddress.ToLower()))
                );
            }

            var originPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Origins.Count != 0)
            {
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
                colorPredicate = colorPredicate.Or(g =>
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Màu sắc"
                        && query.Colors.Any(c => sv.Value.ToLower().Contains(c.ToLower()) || c.ToLower().Contains(sv.Value.ToLower()))
                    )
                );

                colorPredicate = colorPredicate.Or(g
                    => g.GadgetDescriptions.Any(gd
                        => query.Colors.Any(c => gd.Value.Substring(gd.Value.ToLower().IndexOf("màu "), 50).Contains(c.ToLower()))
                    )
                );
            }

            var discountedPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsDiscounted)
            {
                discountedPredicate = discountedPredicate.Or(g =>
                    g.GadgetDiscounts.Any(gd => gd.Status == GadgetDiscountStatus.Active && gd.ExpiredDate > currentDate && gd.DiscountPercentage >= query.MinDiscount
                    && gd.DiscountPercentage <= query.MaxDiscount)
                );
            }

            var availablePredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsAvailable)
            {
                availablePredicate = availablePredicate.Or(g =>
                    g.Quantity > 0 && g.IsForSale == true
                );
            }

            var refreshRatePredicate = PredicateBuilder.New<Gadget>(true);
            if (query.RefreshRates.Count > 0)
            {
                refreshRatePredicate = refreshRatePredicate.And(g =>
                    g.Category.Name == "Laptop" && g.SpecificationValues.Any(
                            sv => sv.SpecificationKey.Name == "Tần số quét" && query.RefreshRates.Any(rr => rr.ToString() == sv.Value)
                        )
                );
            }

            var gadgetPredicate = PredicateBuilder.New<Gadget>(true);
            gadgetPredicate = gadgetPredicate
                                .And(keywordGroupPredicate)
                                .And(pricePredicate)
                                .And(categoryPredicate)
                                .And(brandPredicate)
                                .And(operatingSystemPredicate)
                                .And(storageCapacityLaptopPredicate)
                                .And(storageCapacityPhonePredicate)
                                .And(ramPredicate)
                                .And(locationPredicate)
                                .And(originPredicate)
                                .And(releaseDatePredicate)
                                .And(colorPredicate)
                                .And(discountedPredicate)
                                .And(availablePredicate)
                                .And(refreshRatePredicate)
                                .And(g => g.Status == GadgetStatus.Active && g.Seller.User.Status == UserStatus.Active)
                                ;

            Expression<Func<Gadget, bool>> gadgetExpression = gadgetPredicate;

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

            var sellerNamePredicate = PredicateBuilder.New<Seller>(true);
            if (!string.IsNullOrEmpty(query.SellerName))
            {
                sellerNamePredicate = sellerNamePredicate.And(s => s.ShopName.ToLower().Contains(query.SellerName.ToLower()));
            }

            var sellerPredicate = PredicateBuilder.New<Seller>(true);
            sellerPredicate = sellerPredicate
                                .And(s => s.Gadgets.Any(g => gadgetExpression.Invoke(g)))
                                .And(highRatingPredicate)
                                .And(positiveReviewPredicate)
                                .And(sellerNamePredicate)
                                ;

            var sellers = await context.Sellers.AsExpandable()
                                 .Where(sellerPredicate)
                                 .OrderByDescending(s => query.IsBestSeller
                                            ? s.SellerOrders
                                                .Where(so => so.Status == SellerOrderStatus.Success)
                                                .SelectMany(so => so.SellerOrderItems)
                                                .Sum(soi => soi.GadgetQuantity)
                                            : 0)
                                 .Select(s => new
                                 {
                                     s.Id,
                                     s.ShopName
                                 })
                                 .Take(200)
                                 .ToListAsync();

            var result = new
            {
                query,
                total = sellers.Count,
                sellers
            };

            return Ok(result);

        }
        else
        {
            var keywordGroupPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Keywords.Count > 0)
            {
                var groups = await context.NaturalLanguageKeywordGroups
                                            .Include(g => g.Criteria)
                                                .ThenInclude(c => c.SpecificationKey)
                                            .Include(g => g.Criteria)
                                                .ThenInclude(c => c.Categories)
                                            .Where(g => query.Keywords.Any(k => g.NaturalLanguageKeywords.Any(kk => k.ToLower() == kk.Keyword.ToLower())))
                                            .Where(g => g.Status == NaturalLanguageKeywordGroupStatus.Active)
                                            .ToListAsync();

                //var groups = await context.NaturalLanguageKeywordGroups
                //                            .Include(g => g.Criteria)
                //                                .ThenInclude(c => c.SpecificationKey)
                //                            .Include(g => g.Criteria)
                //                                .ThenInclude(c => c.Categories)
                //                            .Where(g => query.KeywordGroups.Any(kg => kg.ToLower() == g.Name.ToLower()))
                //                            .Where(g => g.Status == NaturalLanguageKeywordGroupStatus.Active)
                //                            .ToListAsync();

                foreach (var group in groups)
                {
                    var groupPredicate = PredicateBuilder.New<Gadget>(true);

                    foreach (var criteria in group.Criteria)
                    {
                        var criteriaPredicate = PredicateBuilder.New<Gadget>(true);
                        var categoryIds = criteria.CriteriaCategories.Select(c => c.CategoryId).ToList();

                        if (criteria.Type == CriteriaType.Name)
                        {
                            criteriaPredicate = criteriaPredicate.Start(g => g.Name.ToLower().Contains(criteria.Contains!.ToLower()));
                            criteriaPredicate = criteriaPredicate.And(g => categoryIds.Contains(g.CategoryId));
                        }

                        if (criteria.Type == CriteriaType.Description)
                        {
                            criteriaPredicate = criteriaPredicate.Start(g => g.GadgetDescriptions.Any(gd => gd.Value.ToLower().Contains(criteria.Contains!.ToLower())));
                            criteriaPredicate = criteriaPredicate.And(g => categoryIds.Contains(g.CategoryId));
                        }

                        if (criteria.Type == CriteriaType.Condition)
                        {
                            criteriaPredicate = criteriaPredicate.Start(g => g.Condition.ToLower().Contains(criteria.Contains!.ToLower()));
                            criteriaPredicate = criteriaPredicate.And(g => categoryIds.Contains(g.CategoryId));
                        }

                        if (criteria.Type == CriteriaType.Price)
                        {

                            criteriaPredicate = criteriaPredicate.Start(g =>
                                effectivePrice.Invoke(g) >= criteria.MinPrice && effectivePrice.Invoke(g) <= criteria.MaxPrice);

                            criteriaPredicate = criteriaPredicate.And(g => categoryIds.Contains(g.CategoryId));
                        }

                        if (criteria.Type == CriteriaType.Specification)
                        {
                            criteriaPredicate = criteriaPredicate.Start(g => g.SpecificationValues
                                                    .Any(sv => sv.SpecificationKeyId == criteria.SpecificationKeyId && sv.Value.ToLower().Contains(criteria.Contains!.ToLower())));
                        }

                        groupPredicate = groupPredicate.Or(criteriaPredicate);
                    }

                    keywordGroupPredicate = keywordGroupPredicate.And(groupPredicate);
                }
            }

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

            var operatingSystemPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.OperatingSystems.Any())
            {
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
                    g.Category.Name == "Điện thoại" &&
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
                    g.Category.Name == "Laptop" &&
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name.StartsWith("Ổ cứng")
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

            var locationPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Locations.Count != 0)
            {
                locationPredicate = locationPredicate.Or(g =>
                    query.Locations.Any(l => g.Seller.ShopAddress.ToLower().Contains(l.ToLower()) || l.ToLower().Contains(g.Seller.ShopAddress.ToLower()))
                );
            }

            var originPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.Origins.Count != 0)
            {
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
                colorPredicate = colorPredicate.Or(g =>
                    g.SpecificationValues.Any(sv =>
                        sv.SpecificationKey.Name == "Màu sắc"
                        && query.Colors.Any(c => sv.Value.ToLower().Contains(c.ToLower()) || c.ToLower().Contains(sv.Value.ToLower()))
                    )
                );

                colorPredicate = colorPredicate.Or(g
                    => g.GadgetDescriptions.Any(gd
                        => query.Colors.Any(c => gd.Value.Substring(gd.Value.ToLower().IndexOf("màu "), 50).Contains(c.ToLower()))
                    )
                );
            }

            var discountedPredicate = PredicateBuilder.New<Gadget>(true);
            if (query.IsDiscounted)
            {
                discountedPredicate = discountedPredicate.Or(g =>
                    g.GadgetDiscounts.Any(gd => gd.Status == GadgetDiscountStatus.Active && gd.ExpiredDate > currentDate && gd.DiscountPercentage >= query.MinDiscount
                    && gd.DiscountPercentage <= query.MaxDiscount)
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

            var refreshRatePredicate = PredicateBuilder.New<Gadget>(true);
            if (query.RefreshRates.Count > 0)
            {
                refreshRatePredicate = refreshRatePredicate.And(g =>
                    g.Category.Name == "Laptop" && g.SpecificationValues.Any(
                            sv => sv.SpecificationKey.Name == "Tần số quét" && query.RefreshRates.Any(rr => rr.ToString() == sv.Value)
                        )
                );
            }

            var outerPredicate = PredicateBuilder.New<Gadget>(true);
            outerPredicate = outerPredicate
                                .And(keywordGroupPredicate)
                                .And(pricePredicate)
                                .And(categoryPredicate)
                                .And(brandPredicate)
                                .And(operatingSystemPredicate)
                                .And(storageCapacityLaptopPredicate)
                                .And(storageCapacityPhonePredicate)
                                .And(ramPredicate)
                                .And(locationPredicate)
                                .And(originPredicate)
                                .And(releaseDatePredicate)
                                .And(colorPredicate)
                                .And(discountedPredicate)
                                .And(highRatingPredicate)
                                .And(positiveReviewPredicate)
                                .And(availablePredicate)
                                .And(refreshRatePredicate)
                                ;

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
                                        .Take(500)
                                        .ToListAsync();

            var result = new
            {
                query,
                total = gadgets.Count,
                gadgets
            };
            return Ok(result);
        }
        return Ok(query);
    }
}
