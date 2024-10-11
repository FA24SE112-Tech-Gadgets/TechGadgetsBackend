using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.SellerApplications.Mappers;
using WebApi.Features.SellerApplications.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.SellerApplications;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager, Role.Seller)]
public class GetSellerApplicationById : ControllerBase
{
    [HttpGet("seller-applications/{sellerApplicationId}")]
    [Tags("Seller Applications")]
    [SwaggerOperation(
        Summary = "Get Seller Application By SellerApplicationId",
        Description = "API is for get seller application detail."
    )]
    [ProducesResponseType(typeof(SellerApplicationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid sellerApplicationId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var sellerApplication = await context.SellerApplications
            .Include(sa => sa.User)
            .Include(sa => sa.BillingMailApplications)
            .FirstOrDefaultAsync(sa => sa.Id == sellerApplicationId)
            ?? throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerApplication", "Không tìm thấy đơn này.")
            .Build();

        var currentUser = await currentUserService.GetCurrentUser();
        if (currentUser!.Role == Role.Seller && sellerApplication.UserId != currentUser.Id)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEA_00)
                .AddReason("role", "Tài khoản không đủ thẩm quyền để truy cập Seller Application này.")
                .Build();
        }

        return Ok(sellerApplication.ToSellerApplicationDetailResponse());
    }
}
