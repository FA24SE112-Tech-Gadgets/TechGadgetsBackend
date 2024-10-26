using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
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

    [HttpPost("natural-languages/process")]
    public async Task<IActionResult> Handler(Request request, NaturalLanguageService naturalLanguageService, AppDbContext context)
    {
        var query = await naturalLanguageService.GetRequestByUserInput(request.Input);
        if (query == null)
        {
            return Ok("natural language query is null");
        }

        var gadgets = await context.Gadgets
                                .OrderBy(g => g.NameVector.L2Distance(query.InputVector!))
                                .Select(g => new
                                {
                                    g.Name
                                })
                                .Take(10)
                                .ToListAsync();

        var result = new
        {
            query,
            d = "xyz"
        };
        return Ok(result);
    }
}