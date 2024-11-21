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
[RequestValidation<Request>]
public class GetGadgetsBySeller : ControllerBase
{
    public new class Request : PagedRequest
    {
        public string? Name { get; set; }
        public SortDir? SortOrder { get; set; }
        public string? SortColumn { get; set; }
    }

    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("gadgets/categories/{categoryId}/sellers/{sellerId}")]
    [Tags("Gadgets")]
    [SwaggerOperation(Summary = "Get Gadgets By Category And Seller",
        Description = """
        This API is for retrieving gadgets by category id and seller id

        `SortColumn` (optional): name, price, createdAt, updatedAt
        """
    )]
    public async Task<IActionResult> Handler(Guid categoryId, Guid sellerId, [FromQuery] Request request, AppDbContext context, CurrentUserService currentUserService)
    {
        var user = await currentUserService.GetCurrentUser();
        var customerId = user?.Customer?.Id;

        if (!await context.Categories.AnyAsync(c => c.Id == categoryId))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("category", "thể loại không tồn tại")
            .Build();
        }

        if (!await context.Sellers.AnyAsync(c => c.Id == sellerId))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("seller", "seller không tồn tại")
            .Build();
        }

        var query = context.Gadgets
            .Include(c => c.Seller)
                .ThenInclude(s => s.User)
            .Include(g => g.FavoriteGadgets)
            .Include(g => g.GadgetDiscounts)
            .Where(g => g.Status == GadgetStatus.Active && g.Seller.User.Status == UserStatus.Active)
            .AsQueryable();

        query = query.OrderByColumn(GetSortProperty(request), request.SortOrder);

        var response = await query
            .Where(g => g.Name.Contains(request.Name ?? ""))
            .Where(g => g.SellerId == sellerId && g.CategoryId == categoryId)
            .Select(g => g.ToGadgetResponse(customerId))
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
            _ => c => c.Id
        };
    }
}
