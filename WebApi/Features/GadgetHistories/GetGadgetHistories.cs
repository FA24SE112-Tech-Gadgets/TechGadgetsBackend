using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.GadgetHistories.Mappers;
using WebApi.Features.GadgetHistories.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.GadgetHistories;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class GetGadgetHistories : ControllerBase
{
    public new class Request : PagedRequest
    {
        public SortByDate SortByDate { get; set; }
    }

    public enum SortByDate
    {
        DESC, ASC
    }

    [HttpGet("gadget-histories")]
    [Tags("Gadget Histories")]
    [SwaggerOperation(
        Summary = "Get List Of Customer Gadget Histories",
        Description = "API is for get list of customer gadget histories. Note:" +
                            "<br>&nbsp; - SortByDate: 'DESC' - Ngày gần nhất, 'ASC' - Ngày xa nhất. Nếu không truyền defaul: 'DESC'"
    )]
    [ProducesResponseType(typeof(PagedList<GadgetHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        // Step 1: Fetch all GadgetHistories for the current user
        var gadgetHistories = await context.GadgetHistories
            .Include(gh => gh.Gadget)
                .ThenInclude(g => g.Brand)
            .Include(gh => gh.Gadget)
                .ThenInclude(g => g.Seller)
                .ThenInclude(s => s.User)
            .Include(gh => gh.Gadget)
                .ThenInclude(g => g.Category)
            .Where(gh => gh.CustomerId == currentUser!.Customer!.Id)
            .ToListAsync();

        // Step 2: Group by GadgetId and select the most recent record
        var groupedHistories = gadgetHistories
            .GroupBy(gh => gh.GadgetId)
            .Select(g => g.OrderByDescending(gh => gh.CreatedAt).First()) // Get the most recent history per GadgetId
            .AsQueryable(); // Convert back to IQueryable for pagination

        // Step 3: Sort by CreatedAt based on request SortByDate
        if (request.SortByDate == SortByDate.ASC)
        {
            groupedHistories = groupedHistories.OrderBy(gh => gh.CreatedAt);
        }
        else
        {
            groupedHistories = groupedHistories.OrderByDescending(gh => gh.CreatedAt);
        }

        // Step 4: Apply manual pagination (skip and take)
        var pagedHistories = groupedHistories
            .Skip(((request.Page ?? 1) - 1) * (request.PageSize ?? 10))
            .Take(request.PageSize ?? 10)
            .ToList();

        var gadgetHistoryResponseList = new PagedList<GadgetHistoryResponse>(
            pagedHistories.Select(gh => gh.ToGadgetHistoryResponse()!).ToList(),
            request.Page ?? 1,
            request.PageSize ?? 10,
            groupedHistories.Count() // Total count for pagination
        );

        return Ok(gadgetHistoryResponseList);
    }
}
