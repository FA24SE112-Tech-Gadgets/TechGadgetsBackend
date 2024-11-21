using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Gadgets.Mappers;
using WebApi.Features.Gadgets.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Gadgets;

[ApiController]
public class GetGadgetByCategoryId : ControllerBase
{
    [HttpGet("gadgets/category/old/{categoryId}")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Get List Gadgets By CategoryId",
        Description = "API is for get list of gadgets by categoryId."
    )]
    [ProducesResponseType(typeof(PagedList<GadgetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] PagedRequest request, [FromRoute] Guid categoryId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (!await context.Categories.AnyAsync(b => b.Id == categoryId))
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("category", "Thể loại không tồn tại")
                        .Build();
        }

        var gadgets = await context.Gadgets
            .Include(c => c.Seller)
                .ThenInclude(s => s.User)
            .Include(c => c.FavoriteGadgets)
            .Include(g => g.GadgetDiscounts)
            .Where(g => g.CategoryId == categoryId && g.Status == GadgetStatus.Active && g.Seller.User.Status == UserStatus.Active)
            .OrderByDescending(g => g.IsForSale)
            .Select(c => c.ToGadgetResponse(currentUser != null ? currentUser.Customer!.Id : null))
            .ToPagedListAsync(request);

        return Ok(gadgets);
    }
}
