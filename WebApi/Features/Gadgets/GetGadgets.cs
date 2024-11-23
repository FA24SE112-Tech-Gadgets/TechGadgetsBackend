using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Gadgets.Mappers;
using WebApi.Features.Gadgets.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Gadgets;

[ApiController]
[RequestValidation<Request>]
public class GetGadgets : ControllerBase
{
    public new class Request : PagedRequest
    {
        public string? Name { get; set; }
        public SortDir? SortOrder { get; set; }
        public string? SortColumn { get; set; }
    }

    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("gadgets")]
    [Tags("Gadgets")]
    [ProducesResponseType(typeof(PagedList<GadgetResponse>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get Gadgets",
        Description = """
        This API is for retrieving gadgets

        `SortColumn` (optional): name, price, createdAt, updatedAt
        """
    )]
    public async Task<IActionResult> Handler([FromQuery] Request request, [FromServices] AppDbContext context, CurrentUserService currentUserService)
    {
        var query = context.Gadgets
                            .Include(c => c.Seller)
                                .ThenInclude(s => s.User)
                            .Include(c => c.FavoriteGadgets)
                            .Include(g => g.GadgetDiscounts)
                            .Where(g => g.Status == GadgetStatus.Active && g.Seller.User.Status == UserStatus.Active)
                            .AsQueryable();

        var user = await currentUserService.GetCurrentUser();
        var customerId = user?.Customer?.Id;

        query = query.OrderByColumn(GetSortProperty(request), request.SortOrder);

        var response = await query
                            .Where(c => c.Name.Contains(request.Name ?? ""))
                            .Select(c => c.ToGadgetResponse(customerId))
                            .ToPagedListAsync(request);

        if (user?.Id != null && !string.IsNullOrEmpty(request.Name))
        {
            var latestKeywordHistory = await context.KeywordHistories
                                            .OrderByDescending(kh => kh.CreatedAt)
                                            .FirstOrDefaultAsync();

            if (latestKeywordHistory == null || latestKeywordHistory.Keyword != request.Name)
            {
                context.KeywordHistories.Add(new KeywordHistory
                {
                    Keyword = request.Name,
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                });

                await context.SaveChangesAsync();
            }
        }

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
