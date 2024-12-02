using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.NaturalLanguageKeywords.Mappers;
using WebApi.Features.NaturalLanguageKeywords.Models;

namespace WebApi.Features.NaturalLanguageKeywords;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
[RequestValidation<Request>]
public class GetNaturalLanguageKeywordGroups : ControllerBase
{
    public new class Request : PagedRequest
    {
        public string? Name { get; set; }
        public SortDir? SortOrder { get; set; }
        public string? SortColumn { get; set; }
        public NaturalLanguageKeywordGroupStatus? Status { get; set; }
    }

    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("natual-language-keyword-groups")]
    [Tags("Natural Language Keywords")]
    [ProducesResponseType(typeof(PagedList<NaturalLanguageKeywordGroupResponse>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get Natural Language Keyword Groups",
        Description = """
        This API is for retrieving natural language keyword groups

        `SortColumn` (optional): name, createdAt, updatedAt
        """
    )]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context)
    {
        var query = context.NaturalLanguageKeywordGroups
                                    .Include(g => g.Criteria.OrderByDescending(c => c.UpdatedAt))
                                        .ThenInclude(c => c.Categories)
                                    .Include(g => g.Criteria.OrderByDescending(c => c.UpdatedAt))
                                        .ThenInclude(c => c.SpecificationKey!.Category)
                                    .Include(g => g.NaturalLanguageKeywords.OrderByDescending(k => k.UpdatedAt))
                                    .AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(g => g.Status == request.Status);
        }

        query = query.OrderByColumn(GetSortProperty(request), request.SortOrder);

        request.Name = request.Name?.ToLower();

        var response = await query
                            .Where(c => c.Name.ToLower().Contains(request.Name ?? ""))
                            .Select(c => c.ToNaturalLanguageKeywordGroupResponse())
                            .ToPagedListAsync(request);

        return Ok(response);
    }

    private static Expression<Func<NaturalLanguageKeywordGroup, object>> GetSortProperty(Request request)
    {
        return request.SortColumn?.ToLower() switch
        {
            "name" => c => c.Name,
            "createdat" => c => c.CreatedAt,
            "updatedat" => c => c.UpdatedAt,
            _ => c => c.UpdatedAt
        };
    }
}
