using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Data;
using WebApi.Features.Payments.Models;

namespace WebApi.Features.Payments;

[ApiController]
public class VnPayPayment : ControllerBase
{
    [HttpGet("webhook/vnpay")]
    [Tags("Webhook")]
    [SwaggerOperation(
            Summary = "Payment Transfer Handler - Webhook API - DO NOT USE!!!",
            Description = "API for VnPay transfer data to Project Server" +
                            "<br>&nbsp; - FE do not use this API. This API is used by VnPay to transfer data to Project Server." +
                            "<br>&nbsp; - VnPay will call this API when the payment is successful."
        )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] VnPayWebhookRequest request, AppDbContext context)
    {
        var walletTracking = await context.WalletTrackings.FirstOrDefaultAsync(wt => wt.PaymentCode == request.Vnp_TxnRef);

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

        return Redirect($"{request.ReturnUrl}?isSuccess={request.IsSuccess}");
    }
}
