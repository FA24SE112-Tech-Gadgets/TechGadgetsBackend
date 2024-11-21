using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Gadgets.Mappers;
using WebApi.Services.Auth;

namespace WebApi.Features.Gadgets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Seller)]
[RequestValidation<Request>]
public class GetGadgetsByCurrentSeller : ControllerBase
{
    public new class Request : PagedRequest
    {
        public string? Name { get; set; }
        public SortDir? SortOrder { get; set; }
        public string? SortColumn { get; set; }
    }

    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("gadgets/category/{categoryId}/current-seller")]
    [Tags("Gadgets")]
    [SwaggerOperation(Summary = "Get Gadgets By Category And Related To Current Seller",
        Description = """
        This API is for retrieving gadgets by category id with related to current seller

        `SortColumn` (optional): name, price, createdAt, updatedAt
        """
    )]
    public async Task<IActionResult> Handler([FromRoute] Guid categoryId, [FromQuery] Request request, [FromServices] AppDbContext context, CurrentUserService currentUserService)
    {
        var user = await currentUserService.GetCurrentUser();

        if (!await context.Categories.AnyAsync(c => c.Id == categoryId))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("category", "thể loại không tồn tại")
            .Build();
        }

        var query = context.Gadgets
            .Include(g => g.FavoriteGadgets)
            .Include(g => g.GadgetDiscounts)
            .AsQueryable();

        query = query.OrderByColumn(GetSortProperty(request), request.SortOrder);

        var response = await query
            .Where(g => g.Name.Contains(request.Name ?? ""))
            .Where(g => g.SellerId == user!.Seller!.Id && g.CategoryId == categoryId)
            .Select(g => g.ToGadgetRelatedToSellerResponse())
            .ToPagedListAsync(request);

        return Ok(response);
    }

    private static Expression<Func<Gadget, object>> GetSortProperty(Request request)
    {
        return request.SortColumn?.ToLower() switch
        {
            "name" => c => c.Name,
            "price" => c => c.Price,
            "createdat" => c => c.CreatedAt,
            "updatedat" => c => c.UpdatedAt,
            _ => c => c.IsForSale
        };
    }
}
