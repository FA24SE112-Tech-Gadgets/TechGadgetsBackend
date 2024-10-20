using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Features.Payments.Models;

namespace WebApi.Features.Payments;

[ApiController]
public class PayOSPayment : ControllerBase
{
    [HttpPost("webhook/payos")]
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
    public async Task<IActionResult> Handler(WebhookType body)
    {
        try
        {
            return Ok(new PaymentResponse(0, "Ok", null));
        }
        catch (Exception)
        {
            return Ok(new PaymentResponse(-1, "fail", null));
        }
    }
}
