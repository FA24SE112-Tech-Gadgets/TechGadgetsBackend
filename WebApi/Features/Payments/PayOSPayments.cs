using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS.Types;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Data;
using WebApi.Features.Payments.Models;

namespace WebApi.Features.Payments;

[ApiController]
public class PayOSPayments : ControllerBase
{
    [HttpPost("webhook/payos")]
    [Tags("Webhook")]
    [SwaggerOperation(
            Summary = "Payment Transfer Handler - Webhook API - DO NOT USE!!!",
            Description = "API for PayOS transfer data to Project Server" +
                            "<br>&nbsp; - FE do not use this API. This API is used by PayOS to transfer data to Project Server." +
                            "<br>&nbsp; - PayOS will call this API when the payment is successful."
        )]
    [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(WebhookType body, AppDbContext context)
    {
        try
        {
            var walletTrackingDetail = await context.WalletTrackings.FirstOrDefaultAsync(wt => wt.PaymentCode == body.data.orderCode.ToString());
            if (walletTrackingDetail != null)
            {
                var userWallet = await context.Wallets.FirstOrDefaultAsync(w => w.Id == walletTrackingDetail.WalletId);
                if (userWallet != null)
                {
                    userWallet.Amount += walletTrackingDetail.Amount;
                    walletTrackingDetail.Status = Data.Entities.WalletTrackingStatus.Success;
                    await context.SaveChangesAsync();
                }
            }
            return Ok(new PaymentResponse(0, "Ok", null));
        }
        catch (Exception)
        {
            return Ok(new PaymentResponse(-1, "fail", null));
        }
    }
}
