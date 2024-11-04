using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.WalletTrackings.Mappers;
using WebApi.Features.WalletTrackings.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.WalletTrackings;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class GetWalletTrackingById : ControllerBase
{
    [HttpGet("wallet-trackings/{walletTrackingId}")]
    [Tags("Wallet Trackings")]
    [SwaggerOperation(
        Summary = "Get Wallet Tracking By WalletTrackingId",
        Description = "This API is for get wallet tracking by walletTrackingId. Note: " +
                            "<br>&nbsp; - Dùng API này để check status nạp tiền Success hay Cancelled'"
    )]
    [ProducesResponseType(typeof(WalletTrackingItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(
        [FromRoute] Guid walletTrackingId,
        AppDbContext context,
        [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var userWallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == currentUser!.Id);

        if (userWallet == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("wallets", "Không tìm thấy ví người dùng.")
            .Build();
        }

        var walletTracking = await context.WalletTrackings
            .Where(wt => wt.Id == walletTrackingId)
            .Select(wt => wt.ToWalletTrackingItemResponse()!)
            .FirstOrDefaultAsync();

        return Ok(walletTracking);
    }
}
