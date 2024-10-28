using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Filters;
using WebApi.Data;
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

        var result = new
        {
            query,
            //gadgets
        };
        return Ok(result);
    }
}