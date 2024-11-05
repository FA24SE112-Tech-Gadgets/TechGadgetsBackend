using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Common.Filters;
using WebApi.Services.AI;

namespace WebApi.Features.Tests;

[ApiController]
[RequestValidation<Request>]
public class IsPositiveContent : ControllerBase
{
    public new class Request
    {
        [Required]
        public string Content { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Content)
                .NotEmpty().WithMessage("Content không được để trống");
        }
    }

    [Tags("Tests")]
    [HttpPost("tests/natural-languages/is-positive-content")]
    public async Task<IActionResult> Handler(Request request, NaturalLanguageService naturalLanguageService)
    {
        var res = await naturalLanguageService.IsPositiveContent(request.Content);
        if (res is null)
        {
            return StatusCode(500, "Lỗi khi xử lí nội dung");
        }

        return Ok(res);
    }
}
