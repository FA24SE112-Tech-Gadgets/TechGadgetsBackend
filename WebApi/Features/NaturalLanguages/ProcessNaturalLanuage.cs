using FluentValidation;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.AI;
using WebApi.Services.Embedding;

namespace WebApi.Features.NaturalLanguages;

[ApiController]
[RequestValidation<Request>]
public class ProcessNaturalLanuage : ControllerBase
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

    private static (int number, string text) ExtractNumberAndString(string input)
    {
        // Find the index where the numeric part ends
        int index = 0;
        while (index < input.Length && char.IsDigit(input[index]))
        {
            index++;
        }

        // Extract the number and the string
        string numberPart = input.Substring(0, index);
        string stringPart = input.Substring(index);

        // Convert the number part to an integer
        int number = int.Parse(numberPart);

        return (number, stringPart);
    }

    [Tags("Natural Language")]
    [HttpPost("natural-languages/process")]
    public async Task<IActionResult> Handler(Request request, NaturalLanguageService naturalLanguageService, EmbeddingService embeddingService, AppDbContext context)
    {
        var query = await naturalLanguageService.GetRequestByUserInput(request.Input);
        if (query == null)
        {
            return Ok("natural language query is null");
        }

        var pricePredicate = PredicateBuilder.New<Gadget>(true);
        pricePredicate = pricePredicate.Or(g => g.Price >= query.MinPrice && g.Price <= query.MaxPrice);

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
                purposePredicate = purposePredicate.Or(g => g.Name.ToLower().Contains("học tập")
                                            || g.Name.ToLower().Contains("sinh viên")
                                            || g.Name.ToLower().Contains("học sinh")
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("học tập"))
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("sinh viên"))
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("học sinh")));
            }

            if (new List<string> { "Làm việc", "Văn phòng" }.Contains(purpose))
            {
                purposePredicate = purposePredicate.Or(g => g.Name.ToLower().Contains("văn phòng")
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("văn phòng"))
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("công sở")));
            }

            if (new List<string> { "Gaming", "Giải trí" }.Contains(purpose))
            {
                purposePredicate = purposePredicate.Or(g => g.Name.ToLower().Contains("gaming")
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("gaming"))
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("giải trí"))
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("chơi game"))
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("liên quân"))
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("liên minh")));
            }

            if (new List<string> { "Vận động", "Ngoài trời", "Thể thao" }.Contains(purpose))
            {
                purposePredicate = purposePredicate.Or(g => g.Name.Contains("sport")
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
                purposePredicate = purposePredicate.Or(g => g.Name.Contains("đồ hoạ")
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
                purposePredicate = purposePredicate.Or(g => g.Name.Contains("nước")
                                            || g.Name.ToLower().Contains("bơi")
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("chống nước"))
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("kháng nước"))
                                            || g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("bơi")));
            }
        }

        var goodBatteryLifePredicate = PredicateBuilder.New<Gadget>(true);
        if (query.IsGoodBatteryLife)
        {
            goodBatteryLifePredicate = goodBatteryLifePredicate.Or(g => g.Category.Name.ToLower() != "điện thoại" ||
                            g.SpecificationValues.Any(sv => sv.SpecificationKey.Name == "Dung lượng pin" && Convert.ToDouble(sv.Value) > 5000));
        }

        var fastChargePredicate = PredicateBuilder.New<Gadget>(true);
        if (query.IsFastCharge)
        {
            var fastChargeVector = await embeddingService.GetEmbedding("sạc nhanh");

            fastChargePredicate = fastChargePredicate.Or(g => g.SpecificationValues.Any(sv => sv.SpecificationKey.Name == "Tính năng" &&
                                    (1 - sv.Vector.CosineDistance(fastChargeVector)) > 0.7));
        }

        var wideScreenPredicate = PredicateBuilder.New<Gadget>(true);
        if (query.IsWideScreen)
        {
            wideScreenPredicate = wideScreenPredicate.Or(g =>
                        g.Category.Name != "Laptop" ||
                        g.SpecificationValues
                             .Any(sv => sv.SpecificationKey.Name == "Màn hình" && Convert.ToDouble(sv.Value) >= 16));
        }

        var foldablePredicate = PredicateBuilder.New<Gadget>(true);
        if (query.IsFoldable)
        {
            foldablePredicate = foldablePredicate.Or(g =>
                    g.Category.Name != "Điện thoại" ||
                    g.GadgetDescriptions.Any(desc => desc.Value.ToLower().Contains("gập")
                                                    || desc.Value.ToLower().Contains("công nghệ gập")
                                                    || desc.Value.ToLower().Contains("thiết kế gập")
                                                    || desc.Value.ToLower().Contains("màn hình gập")
                                                    || desc.Value.ToLower().Contains("điện thoại gập")));
        }

        var inchPredicate = PredicateBuilder.New<Gadget>(true);
        if (query.Categories.Contains("Điện thoại"))
        {
            inchPredicate = inchPredicate.Or(g =>
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
        if (query.IsHighResolution && query.Categories.Contains("Điện thoại"))
        {
            var phoneHighResolutionVectors = await embeddingService.GetEmbeddings(["Full HD", "Retina", "2K", "1.5K"]);

            highResolutionPredicate = highResolutionPredicate.Or(g =>
                g.Category.Name != "Điện thoại" ||
                g.SpecificationValues.Any(sv =>
                    sv.SpecificationKey.Name == "Độ phân giải màn hình"
                    && phoneHighResolutionVectors.Any(vector =>
                            (1 - sv.Vector.CosineDistance(vector)) >= 0.5)
                    ));
        }
        if (query.IsHighResolution && query.Categories.Contains("Laptop"))
        {
            var laptopHighResolutionVectors = await embeddingService.GetEmbeddings(["Full HD", "QHD", "2.8K", "WUXGA", "2.5K", "4K", "3.2K", "Retina"]);

            highResolutionPredicate = highResolutionPredicate.Or(g =>
                g.Category.Name != "Laptop" ||
                g.SpecificationValues.Any(sv =>
                    sv.SpecificationKey.Name == "Độ phân giải"
                    && laptopHighResolutionVectors.Any(vector => (1 - sv.Vector.CosineDistance(vector)) >= 0.5)
                    ));
        }

        var operatingSystemPredicate = PredicateBuilder.New<Gadget>(true);
        List<Vector> operatingSystemVectors = [];
        if (query.OperatingSystems.Any())
        {
            operatingSystemVectors = await embeddingService.GetEmbeddings(query.OperatingSystems);
            operatingSystemPredicate = operatingSystemPredicate.Or(g =>
                g.Category.Name != "Laptop" ||
                g.SpecificationValues.Any(sv =>
                    sv.SpecificationKey.Name == "Hệ điều hành"
                    && operatingSystemVectors.Any(vector => (1 - sv.Vector.CosineDistance(vector)) >= 0.6)
            ));

            operatingSystemPredicate = operatingSystemPredicate.And(g =>
                g.Category.Name != "Điện thoại" ||
                g.SpecificationValues.Any(sv =>
                    sv.SpecificationKey.Name == "Hệ điều hành"
                    && operatingSystemVectors.Any(vector => (1 - sv.Vector.CosineDistance(vector)) >= 0.6)
            ));
        }

        var storageCapacityPhonePredicate = PredicateBuilder.New<Gadget>(true);
        if (query.StorageCapacitiesPhone.Any())
        {
            List<(int number, string text)> extractedStoragePhone = query.StorageCapacitiesPhone.Select(ExtractNumberAndString).ToList();

            storageCapacityPhonePredicate = storageCapacityPhonePredicate.Or(g =>
                g.Category.Name != "Điện thoại" ||
                g.SpecificationValues.Any(sv =>
                    sv.SpecificationKey.Name == "Dung lượng lưu trữ"
                    && extractedStoragePhone.Any(item => sv.Value == item.number.ToString() && sv.SpecificationUnit.Name == item.text)
            ));
        }

        //var category = await context.Categories.FirstOrDefaultAsync(c => c.Name.Equals(query.Categories[0], StringComparison.CurrentCultureIgnoreCase));

        //var gadgets = await context.Gadgets
        //                        .Where(g => g.Category == category)
        //                        .OrderBy(g => g.NameVector.L2Distance(query.InputVector!))
        //                        .Select(g => new
        //                        {
        //                            g.Name,
        //                            Distance = g.NameVector.L2Distance(query.InputVector!)
        //                        })
        //                        .Take(10)
        //                        .ToListAsync();

        var outerPredicate = PredicateBuilder.New<Gadget>(true);
        outerPredicate = outerPredicate
                        .And(pricePredicate)
                        .And(categoryPredicate)
                        .And(brandPredicate)
                        .And(purposePredicate)
                        .And(goodBatteryLifePredicate)
                        .And(fastChargePredicate)
                        .And(wideScreenPredicate)
                        .And(foldablePredicate)
                        .And(inchPredicate)
                        .And(highResolutionPredicate)
                        .And(operatingSystemPredicate)
                        //.And(storageCapacityPhonePredicate)
                        ;

        var gadgets = context.Gadgets
                                .AsExpandable()
                                .Where(outerPredicate)
                                .Select(g => new
                                {
                                    g.Id,
                                    g.Name,
                                })
                                .Take(500)
                                .ToList();

        var result = new
        {
            query,
            total = gadgets.Count,
            gadgets
        };
        return Ok(result);
    }
}