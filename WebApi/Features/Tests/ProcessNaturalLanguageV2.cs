using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Filters;
using WebApi.Data;
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
        var query = await naturalLanguageService.GetRequestByUserInput(request.Input);
        if (query == null)
        {
            return StatusCode(500, "Lỗi khi xử lí lệnh ngôn ngữ tự nhiên.");
        }

        return Ok(query);
    }
}
