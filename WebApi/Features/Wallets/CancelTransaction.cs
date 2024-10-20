﻿using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Wallets.Models;
using WebApi.Services.Auth;
using WebApi.Services.Payment.Models;
using WebApi.Services.Payment;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Features.Wallets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class CancelTransaction : ControllerBase
{
    [HttpPost("wallet-trackings/{walletTrackingId}/cancel")]
    [Tags("Wallet Trackings")]
    [SwaggerOperation(
        Summary = "Create Wallet Deposit",
        Description = "This API is for create wallet deposit. Note: " +
                            "<br>&nbsp; - PaymentMethod: VnPay, Momo, PayOS." +
                            "<br>&nbsp; - Amount phải tối thiểu là 2,000 và tối đa 50,000,000." +
                            "<br>&nbsp; - Return Url là web của FE." +
                            "<br>&nbsp; - Nếu đang có 1 giao dịch Pending thì cần phải Cancel hoặc Success giao dịch đó trước khi tiến hành tạo giao dịch mới"
    )]
    [ProducesResponseType(typeof(DepositResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(
        [FromRoute] Guid walletTrackingId,
        AppDbContext context,
        [FromServices] CurrentUserService currentUserService,
        [FromServices] MomoPaymentService momoPaymentService,
        [FromServices] VnPayPaymentService vnPayPaymentService,
        [FromServices] PayOSPaymentSerivce payOSPaymentSerivce)
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

        var walletTrackingDetail = await context.WalletTrackings
            .FirstOrDefaultAsync(wt => wt.WalletId == userWallet.Id && wt.Id == walletTrackingId);

        if (walletTrackingDetail == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("walletTrackings", "Không tìm thấy giao dịch này.")
            .Build();
        }

        //if (walletTrackingDetail.Status)
        
        return Ok();
    }
}
