using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.SpecificationKeys.Mappers;
using WebApi.Features.SpecificationKeys.Models;

namespace WebApi.Features.SpecificationKeys;

[ApiController]
[RequestValidation<Request>]
public class GetSpecificationKeysByCategory : ControllerBase
{
    public new class Request : PagedRequest
    {
        public string? Name { get; set; }
        public SortDir? SortOrder { get; set; }
        public string? SortColumn { get; set; }
    }
    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("specification-keys/categories/{categoryId}")]
    [Tags("Specification Keys")]
    [ProducesResponseType(typeof(PagedList<SpecificationKeyResponse>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get Specification Keys By Category Id",
        Description = """
        This API is for retrieving speficication keys by category id

        `SortColumn` (optional): name
        """
    )]
    public async Task<IActionResult> Handler(Guid categoryId, [FromQuery] Request request, [FromServices] AppDbContext context)
    {
        if (!await context.Categories.AnyAsync(c => c.Id == categoryId))
        {
            throw TechGadgetException
                        .NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("category", "Thể loại không tồn tại")
                        .Build();
        }

        var query = context.SpecificationKeys
                           .Include(c => c.SpecificationUnits)
                           .AsQueryable();

        query = query.OrderByColumn(GetSortProperty(request), request.SortOrder);

        var response = await query
                             .Where(c => c.Name.Contains(request.Name ?? ""))
                             .Where(c => c.CategoryId == categoryId)
                             .Select(c => c.ToSpecificationKeyResponse())
                             .ToPagedListAsync(request);

        return Ok(response);
    }


    private static Expression<Func<SpecificationKey, object>> GetSortProperty(Request request)
    {
        return request.SortColumn?.ToLower() switch
        {
            "name" => c => c.Name,
            _ => c => c.Id
        };
    }
}
