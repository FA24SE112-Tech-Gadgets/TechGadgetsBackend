using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.KeywordHistories.Models;
using WebApi.Features.NaturalLanguagePrompts.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.NaturalLanguagePrompts;

[ApiController]
[JwtValidation]
[RequestValidation<Request>]
public class GetNaturalLanguagePrompts : ControllerBase
{
    public new class Request : PagedRequest
    {
        public string? Prompt { get; set; }
        public SortDir? SortOrder { get; set; }
        public string? SortColumn { get; set; }
    }

    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("natural-language-prompts")]
    [Tags("Natural Language Prompts")]
    [ProducesResponseType(typeof(PagedList<KeywordHistoryResponse>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get Natural Language Prompts",
        Description = """
        `SortColumn` (optional): prompt, createdAt, updatedAt
        """
    )]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, CurrentUserService currentUserService)
    {
        var query = context.NaturalLanguagePrompts
                                    .AsQueryable();


        query = query.OrderByColumn(GetSortProperty(request), request.SortOrder);

        request.Prompt = request.Prompt?.ToLower();

        var response = await query
                            .Where(c => c.Prompt.ToLower().Contains(request.Prompt ?? ""))
                            .Select(c => new NaturalLanguagePromptResponse
                            {
                                Prompt = c.Prompt,
                                Id = c.Id,
                            })
                            .ToPagedListAsync(request);

        return Ok(response);
    }

    private static Expression<Func<NaturalLanguagePrompt, object>> GetSortProperty(Request request)
    {
        return request.SortColumn?.ToLower() switch
        {
            "prompt" => c => c.Prompt,
            "createdat" => c => c.CreatedAt,
            "updatedat" => c => c.UpdatedAt,
            _ => c => c.UpdatedAt
        };
    }
}

