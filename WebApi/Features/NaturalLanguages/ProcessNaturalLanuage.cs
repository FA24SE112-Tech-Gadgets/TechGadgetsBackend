using FluentValidation;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.AI;

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

    [Tags("Natural Language")]
    [HttpPost("natural-languages/process")]
    public async Task<IActionResult> Handler(Request request, NaturalLanguageService naturalLanguageService, AppDbContext context)
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
                            g.SpecificationValues.Any(sv => sv.SpecificationKey.Name == "Dung lượng pin" && sv.Value.CompareTo("5000") > 0));
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
                        .And(goodBatteryLifePredicate);

        var gadgets = context.Gadgets
                                .AsExpandable()
                                .Where(outerPredicate)
                                .Select(g => new
                                {
                                    g.Id,
                                    g.Name
                                })
                                .Take(10)
                                .ToList();

        var result = new
        {
            query,
            gadgets
        };
        return Ok(result);
    }
}