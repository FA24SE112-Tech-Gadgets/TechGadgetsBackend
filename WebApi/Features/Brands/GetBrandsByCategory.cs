using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Brands.Mappers;

namespace WebApi.Features.Brands;

[ApiController]
[RequestValidation<Request>]
public class GetBrandsByCategory : ControllerBase
{
    public new class Request : PagedRequest
    {
        public string? Name { get; set; }
        public SortDir? SortOrder { get; set; }
        public string? SortColumn { get; set; }
    }

    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("brands/categories/{categoryId}")]
    [Tags("Brands")]
    [SwaggerOperation(Summary = "Get Brands By Category Id",
        Description = """
        This API is for retrieving brands by category id

        `SortColumn` (optional): name
        """
    )]
    public async Task<IActionResult> Handler(Guid categoryId, [FromQuery] Request request, [FromServices] AppDbContext context)
    {
        if (!await context.Categories.AnyAsync(c => c.Id == categoryId))
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("category", "thể loại không tồn tại")
                        .Build();
        }

        var query = context.Brands.AsQueryable();

        query = query.OrderByColumn(GetSortProperty(request), request.SortOrder);

        var response = await query
                            .Where(b => b.Name.Contains(request.Name ?? ""))
                            .Where(b => b.Categories.Any(c => c.Id == categoryId))
                            .Select(b => b.ToBrandResponse())
                            .ToPagedListAsync(request);

        return Ok(response);
    }

    private static Expression<Func<Brand, object>> GetSortProperty(Request request)
    {
        return request.SortColumn?.ToLower() switch
        {
            "name" => c => c.Name,
            _ => c => c.Id
        };
    }
}
