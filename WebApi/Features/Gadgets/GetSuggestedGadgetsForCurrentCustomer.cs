using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector;
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
[JwtValidation]
[RolesFilter(Role.Customer)]
public class GetSuggestedGadgetsForCurrentCustomer : ControllerBase
{
    public new class Request : PagedRequest;
    public class Validator : PagedRequestValidator<Request>;

    [HttpGet("gadgets/suggested/current-customer")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Get List Of Suggested Gadgets For Current Customer",
        Description = "API is for get list of suggested gadgets."
    )]
    [ProducesResponseType(typeof(PagedList<GadgetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, CurrentUserService currentUserService)
    {
        var user = await currentUserService.GetCurrentUser();
        var customerId = user?.Customer?.Id;

        var purchasedGadgetVectors = await context.SellerOrderItems
                                    .Where(soi => soi.SellerOrder.Status == SellerOrderStatus.Success
                                                   && soi.SellerOrder.Order.CustomerId == customerId
                                                   && soi.SellerOrder.CreatedAt >= DateTime.UtcNow.AddDays(-30))
                                    .Select(soi => soi.Gadget.Vector!.ToArray())
                                    .Distinct()
                                    .ToListAsync();

        var favoriteGadgetVectors = await context.FavoriteGadgets
                                        .Where(fg => fg.CustomerId == customerId && fg.CreatedAt >= DateTime.UtcNow.AddDays(-30))
                                        .Select(fg => fg.Gadget.Vector!.ToArray())
                                        .ToListAsync();

        var historyGadgetVectors = await context.GadgetHistories
                                         .Where(gh => gh.CustomerId == customerId && gh.CreatedAt >= DateTime.UtcNow.AddDays(-30))
                                         .Select(gh => gh.Gadget.Vector!.ToArray())
                                         .Distinct()
                                         .ToListAsync();

        var finalVector = CalculateAverage([.. purchasedGadgetVectors, .. favoriteGadgetVectors, .. historyGadgetVectors]);

        var query = context.Gadgets
                            .Include(c => c.Seller)
                                .ThenInclude(s => s.User)
                            .Include(c => c.FavoriteGadgets)
                            .Include(g => g.GadgetDiscounts)
                            .Where(g => g.Status == GadgetStatus.Active && g.Seller.User.Status == UserStatus.Active && g.IsForSale == true)
                            .OrderByDescending(g => finalVector.Length != 0 ? 1 - g.Vector!.CosineDistance(new Vector(finalVector)) : 0)
                            .AsQueryable();

        var response = await query
                            .Select(c => c.ToGadgetResponse(customerId))
                            .ToPagedListAsync(request);

        return Ok(response);
    }

    private static float[] CalculateAverage(List<float[]> vectors)
    {
        if (vectors == null || vectors.Count == 0)
            return [];

        int vectorLength = 1536;
        float[] sum = new float[vectorLength];

        // Sum the vectors
        foreach (var vector in vectors)
        {
            if (vector.Length != vectorLength)
                continue;

            for (int i = 0; i < vectorLength; i++)
            {
                sum[i] += vector[i];
            }
        }

        // Calculate the average
        float[] average = new float[vectorLength];
        for (int i = 0; i < vectorLength; i++)
        {
            average[i] = sum[i] / vectors.Count;
        }

        return average;
    }
}
