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

    [HttpGet("gadgets/sellers/{sellerId}")]
    [Tags("Gadgets")]
    [SwaggerOperation(Summary = "Get Gadgets By Seller Id",
        Description = """
        This API is for retrieving gadgets by seller id

        `SortColumn` (optional): name, price, createdAt, updatedAt
        """
    )]
    public async Task<IActionResult> Handler(Guid sellerId, [FromQuery] Request request, [FromServices] AppDbContext context, CurrentUserService currentUserService)
    {
        if (!await context.Sellers.AnyAsync(s => s.Id == sellerId))
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("seller", "Seller không tồn tại")
                        .Build();
        }

        var query = context.Gadgets
                            .Include(c => c.Seller)
                                .ThenInclude(s => s.User)
                            .Include(c => c.FavoriteGadgets)
                            .Include(g => g.GadgetDiscounts)
                            .AsQueryable();

        var user = await currentUserService.GetCurrentUser();
        var customerId = user?.Customer?.Id;

        query = query.OrderByColumn(GetSortProperty(request), request.SortOrder);

        var response = await query
                            .Where(c => c.Name.Contains(request.Name ?? ""))
                            .Where(c => c.SellerId == sellerId)
                            .Select(c => c.ToGadgetResponse(customerId))
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
