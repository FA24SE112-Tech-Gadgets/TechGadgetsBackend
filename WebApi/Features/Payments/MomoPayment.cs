using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Payments.Models;

namespace WebApi.Features.Payments;

[ApiController]
public class MomoPayment : ControllerBase
{
    [HttpGet("webhook/momo")]
    [Tags("Webhook")]
    [SwaggerOperation(
            Summary = "Payment Transfer Handler - Webhook API - DO NOT USE!!!",
            Description = "API for Momo transfer data to Project Server" +
                            "<br>&nbsp; - FE do not use this API. This API is used by Momo to transfer data to Project Server." +
                            "<br>&nbsp; - Momo will call this API when the payment is successful."
        )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] MomoWebhookRequest request, AppDbContext context)
    {
        var walletTracking = await context.WalletTrackings.FirstOrDefaultAsync(wt => wt.PaymentCode == request.OrderId);

        if (walletTracking != null && request.IsSuccess && walletTracking.CreatedAt.AddMinutes(5) >= DateTime.UtcNow)
        {
            var userWallet = await context.Wallets.FirstOrDefaultAsync(w => w.Id == walletTracking.WalletId);
            if (userWallet != null)
            {
                userWallet.Amount += walletTracking.Amount;
                walletTracking.Status = Data.Entities.WalletTrackingStatus.Success;
                walletTracking.DepositedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
        }
        if (walletTracking != null && !request.IsSuccess)
        {
            walletTracking.Status = WalletTrackingStatus.Cancelled;
            await context.SaveChangesAsync();
        }

        return Redirect($"{request.ReturnUrl}");
    }
}
