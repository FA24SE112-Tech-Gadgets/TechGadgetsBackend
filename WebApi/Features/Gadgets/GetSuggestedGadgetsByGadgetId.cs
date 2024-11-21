using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
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
[RequestValidation<Request>]
public class GetSuggestedGadgetsByGadgetId : ControllerBase
{
    public new class Request : PagedRequest;
    public class Validator : PagedRequestValidator<Request>;

    [HttpGet("gadgets/suggested/{gadgetId}")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Get List Of Suggested Gadgets By Gadget Id",
        Description = "API is for get list of suggested gadgets."
    )]
    [ProducesResponseType(typeof(PagedList<GadgetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, Guid gadgetId, AppDbContext context, CurrentUserService currentUserService)
    {
        var sourceGadget = await context.Gadgets.FirstOrDefaultAsync(g => g.Id == gadgetId);
        if (sourceGadget is null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("gadget", $"Sản phẩm {gadgetId} không tồn tại.")
            .Build();
        }
        var query = context.Gadgets
                            .Include(c => c.Seller)
                                .ThenInclude(s => s.User)
                            .Include(c => c.FavoriteGadgets)
                            .Include(g => g.GadgetDiscounts)
                            .Where(g => g.Status == GadgetStatus.Active && g.Seller.User.Status == UserStatus.Active && g.Id != gadgetId && g.IsForSale == true
                                        && g.CategoryId == sourceGadget.CategoryId)
                            .OrderByDescending(g => 1 - g.Vector!.CosineDistance(sourceGadget.Vector!))
                            .AsQueryable();

        var user = await currentUserService.GetCurrentUser();
        var customerId = user?.Customer?.Id;

        var response = await query
                            .Select(c => c.ToGadgetResponse(customerId))
                            .ToPagedListAsync(request);

        return Ok(response);
    }
}
