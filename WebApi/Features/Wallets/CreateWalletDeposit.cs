﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Wallets.Models;
using WebApi.Services.Auth;
using WebApi.Services.Payment;
using WebApi.Services.Payment.Models;

namespace WebApi.Features.Wallets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
[RequestValidation<Request>]
public class CreateWalletDeposit : ControllerBase
{
    public new class Request
    {
        public int Amount { get; set; }
        public string? Info { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string ReturnUrl { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(sp => sp.Amount)
                .GreaterThan(2000)
                .WithMessage("Số tiền phải lớn hơn 2,000.")
                .LessThan(50000000)
                .WithMessage("Số tiền phải nhỏ hơn 50,000,000.");
            RuleFor(sp => sp.PaymentMethod)
                .IsInEnum()
                .WithMessage("Phương thức thanh toán không hợp lệ.");
            RuleFor(sp => sp.ReturnUrl)
                .NotEmpty()
                .WithMessage("ReturnUrl không được để trống.");
        }
    }

    [HttpPost("wallet/deposit")]
    [Tags("Wallets")]
    [SwaggerOperation(
        Summary = "Create Wallet Deposit",
        Description = "This API is for create wallet deposit. Note: " +
                            "<br>&nbsp; - PaymentMethod: VnPay, Momo, PayOS." +
                            "<br>&nbsp; - Amount phải tối thiểu là 2,000 và tối đa 50,000,000." +
                            "<br>&nbsp; - Return Url là web của FE."
    )]
    [ProducesResponseType(typeof(DepositResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(
        [FromBody] Request request,
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

        WalletTracking walletTracking = new WalletTracking()
        {
            WalletId = userWallet!.Id,
            PaymentMethod = request.PaymentMethod,
            Amount = request.Amount,
            Type = WalletTrackingType.Deposit,
            Status = WalletTrackingStatus.Pending,
            CreatedAt = DateTime.UtcNow,
        }!;

        DepositResponse depositResponse = new DepositResponse()!;

        switch (request.PaymentMethod)
        {
            case PaymentMethod.VnPay:
                string vnPayPaymentCode = Guid.NewGuid().ToString();
                walletTracking.PaymentCode = vnPayPaymentCode;
                VnPayPayment vnPayPayment = new VnPayPayment()
                {
                    PaymentReferenceId = vnPayPaymentCode,
                    Amount = request.Amount,
                    Info = request.Info,
                    returnUrl = request.ReturnUrl,
                }!;
                try
                {
                    depositResponse.DepositUrl = await vnPayPaymentService.CreatePaymentAsync(vnPayPayment);
                }
                catch (Exception ex)
                {
                    throw TechGadgetException.NewBuilder()
                    .WithCode(TechGadgetErrorCode.WES_00)
                    .AddReason("VnPay", ex.Message)
                    .Build();
                }
                break;
            case PaymentMethod.Momo:
                string momoPaymentCode = Guid.NewGuid().ToString();
                walletTracking.PaymentCode = momoPaymentCode;
                MomoPayment momoPayment = new MomoPayment()
                {
                    PaymentReferenceId = momoPaymentCode,
                    Amount = request.Amount,
                    Info = request.Info,
                    returnUrl = request.ReturnUrl,
                }!;
                try
                {
                    depositResponse.DepositUrl = await momoPaymentService.CreatePaymentAsync(momoPayment);
                }
                catch (Exception ex)
                {
                    throw TechGadgetException.NewBuilder()
                    .WithCode(TechGadgetErrorCode.WES_00)
                    .AddReason("Momo", ex.Message)
                    .Build();
                }
                break;
            case PaymentMethod.PayOS:
                long payOSPaymentCode = GenerateDailyRandomLong();
                walletTracking.PaymentCode = payOSPaymentCode.ToString();
                PayOSPayment payOSPayment = new PayOSPayment()
                {
                    PaymentReferenceId = payOSPaymentCode,
                    Amount = request.Amount,
                    Info = request.Info,
                    ReturnUrl = request.ReturnUrl,
                }!;
                try
                {
                    depositResponse.DepositUrl = await payOSPaymentSerivce.CreatePaymentAsync(payOSPayment);
                }
                catch (Exception ex)
                {
                    throw TechGadgetException.NewBuilder()
                    .WithCode(TechGadgetErrorCode.WES_00)
                    .AddReason("PayOS", ex.Message)
                    .Build();
                }
                break;
            default:
                break;
        }
        await context.WalletTrackings.AddAsync(walletTracking);
        await context.SaveChangesAsync();

        return Ok(depositResponse);
    }

    private static long GenerateDailyRandomLong()
    {
        // Lấy ngày hiện tại (chỉ lấy phần ngày)
        DateTime today = DateTime.Today;

        // Tạo một seed từ ngày hôm nay bằng cách sử dụng tổng các thành phần ngày
        int seed = today.Year * 10000 + today.Month * 100 + today.Day;

        // Sử dụng seed để tạo Random (mỗi ngày sẽ có seed khác nhau)
        Random random = new Random(seed);

        // Tạo số ngẫu nhiên từ 100000 đến 999999
        long randomLong = random.Next(100000, 1000000);

        return randomLong;
    }
}
