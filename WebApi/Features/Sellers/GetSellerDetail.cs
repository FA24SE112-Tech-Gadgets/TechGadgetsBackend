using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.SellerApplications.Models;
using WebApi.Features.Sellers.Mappers;
using WebApi.Services.Auth;

namespace WebApi.Features.Sellers;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Seller)]
public class GetSellerDetail : ControllerBase
{
    [HttpGet("seller/current")]
    [Tags("Sellers")]
    [SwaggerOperation(
        Summary = "Get Current Seller Information",
        Description = "API is for get current seller detail."
    )]
    [ProducesResponseType(typeof(SellerApplicationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Seller is null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("seller", "Seller chưa được kích hoạt")
                .Build();
        }

        var seller = await context.Sellers
            .Include(s => s.User)
            .Include(s => s.BillingMails)
            .FirstOrDefaultAsync(s => s.UserId == currentUser!.Id)
            ?? throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("seller", "Không tìm seller này.")
            .Build();

        return Ok(seller.ToSellerDetailResponse());
    }
}
