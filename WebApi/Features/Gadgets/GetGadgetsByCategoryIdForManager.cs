using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Gadgets.Mappers;
using WebApi.Features.Gadgets.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Gadgets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class GetGadgetsByCategoryIdForManager : ControllerBase
{
    public new class Request : PagedRequest
    {
        public string? Name { get; set; }
        public List<Guid>? Brands { get; set; }
        public GadgetStatus? GadgetStatus { get; set; }
    }

    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("gadgets/category/{categoryId}/managers")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Get List Gadgets By CategoryId For Manager",
        Description = "API is for get list of gadgets by categoryId for manager"
    )]
    [ProducesResponseType(typeof(PagedList<GadgetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, [FromRoute] Guid categoryId, AppDbContext context, CurrentUserService currentUserService)
    {
        if (!await context.Categories.AnyAsync(b => b.Id == categoryId))
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("category", "Thể loại không tồn tại")
                        .Build();
        }

        var query = context.Gadgets
            .Include(g => g.Seller)
                .ThenInclude(s => s.User)
            .Include(g => g.FavoriteGadgets)
            .Include(g => g.GadgetDiscounts)
            .Include(g => g.SpecificationValues)
            .Where(g => g.CategoryId == categoryId)
            .Where(c => c.Name.Contains(request.Name ?? ""))
            .AsQueryable();

        if (request.GadgetStatus.HasValue)
        {
            query = query.Where(g => g.Status == request.GadgetStatus);
        }

        if (request.Brands != null && request.Brands.Count > 0)
        {
            query = query.Where(g => request.Brands.Contains(g.BrandId));
        }

        var gadgets = await query.ToListAsync();

        int page = request.Page == null ? 1 : (int)request.Page;
        int pageSize = request.PageSize == null ? 10 : (int)request.PageSize;
        int skip = (page - 1) * pageSize;


        var gadgetResponse = gadgets.ToListGadgetsResponse(null);
        var response = new PagedList<GadgetResponse>(
            gadgetResponse!.Skip(skip).Take(pageSize).ToList(),
            page,
            pageSize,
            gadgetResponse!.Count
        );

        var user = await currentUserService.GetCurrentUser();

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
}

