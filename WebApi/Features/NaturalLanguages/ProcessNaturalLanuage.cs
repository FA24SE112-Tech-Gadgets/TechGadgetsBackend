using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Filters;
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
    public async Task<IActionResult> Handler(Request request, NaturalLanguageService naturalLanguageService)
    {
        var query = await naturalLanguageService.GetRequestByUserInput(request.Input);
        if (query == null)
        {
            return Ok("natural language query is null");
        }
        return Ok(query);
    }
}