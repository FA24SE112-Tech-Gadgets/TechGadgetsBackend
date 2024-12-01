using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Gadgets.Mappers;
using WebApi.Features.Gadgets.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Gadgets;

[ApiController]
public class GetGadgetById : ControllerBase
{
    [HttpGet("gadgets/{id}")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Get Gadget by Id",
        Description = "API is for get gadget detail."
    )]
    [ProducesResponseType(typeof(GadgetDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, AppDbContext context, CurrentUserService currentUserService)
    {
        var gadget = await context.Gadgets
                                    .Include(g => g.Seller.User)
                                    .Include(g => g.Brand)
                                    .Include(g => g.Category)
                                    .Include(g => g.SpecificationValues)
                                        .ThenInclude(sv => sv.SpecificationKey)
                                    .Include(g => g.SpecificationValues)
                                        .ThenInclude(sv => sv.SpecificationUnit)
                                    .Include(g => g.GadgetDescriptions)
                                    .Include(g => g.GadgetImages)
                                    .Include(g => g.FavoriteGadgets)
                                    .Include(g => g.GadgetDiscounts)
                                    .FirstOrDefaultAsync(g => g.Id == id);

        if (gadget is null)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("gadget", "Sản phẩm không tồn tại")
                        .Build();
        }

        var user = await currentUserService.GetCurrentUser();
        if (user is not null && user.Customer is not null)
        {
            var latest = await context.GadgetHistories
                                    .OrderByDescending(gh => gh.CreatedAt)
                                    .FirstOrDefaultAsync();

            if (latest == null || latest.GadgetId != id)
            {
                context.GadgetHistories.Add(new GadgetHistory
                {
                    GadgetId = id,
                    Customer = user.Customer,
                    CreatedAt = DateTime.UtcNow,
                });

                await context.SaveChangesAsync();
            }
        }

        var customerId = user?.Customer?.Id;

        return Ok(gadget.ToGadgetDetailResponse(customerId));
    }
}
