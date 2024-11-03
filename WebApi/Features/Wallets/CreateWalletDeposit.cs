using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Wallets.Models;
using WebApi.Services.Auth;
using WebApi.Services.Notifications;
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
                .GreaterThanOrEqualTo(2000)
                .WithMessage("Số tiền phải lớn hơn 2,000.")
                .LessThanOrEqualTo(50000000)
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
                            "<br>&nbsp; - Dùng API này để nạp tiền vào ví." +
                            "<br>&nbsp; - PaymentMethod: VnPay, Momo, PayOS." +
                            "<br>&nbsp; - Amount phải tối thiểu là 2,000 và tối đa 50,000,000." +
                            "<br>&nbsp; - Return Url là web của FE." +
                            "<br>&nbsp; - Nếu đang có 1 giao dịch Pending thì cần phải Cancel hoặc Success giao dịch đó trước khi tiến hành tạo giao dịch mới" +
                            "<br>&nbsp; - User bị Inactive thì nạp tiền vào ví được."
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
        [FromServices] PayOSPaymentSerivce payOSPaymentSerivce,
        [FromServices] FCMNotificationService fcmNotificationService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        var userWallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == currentUser!.Id);

        if (userWallet == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("wallets", "Không tìm thấy ví người dùng.")
            .Build();
        }

        bool isExist = await context.WalletTrackings.AnyAsync(wt => wt.WalletId == userWallet!.Id && WalletTrackingStatus.Pending == wt.Status);
        if (isExist)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_01)
            .AddReason("walletTrackings", "Bạn đang có 1 giao dịch đang chờ thanh toán trước đó.")
            .Build();
        }

        DateTime createdAt = DateTime.UtcNow;
        Guid walletTrackingId = Guid.NewGuid();
        WalletTracking walletTracking = new WalletTracking()
        {
            Id = walletTrackingId,
            WalletId = userWallet!.Id,
            PaymentMethod = request.PaymentMethod,
            Amount = request.Amount,
            Type = WalletTrackingType.Deposit,
            Status = WalletTrackingStatus.Pending,
            CreatedAt = createdAt,
        }!;

        DepositResponse depositResponse = new DepositResponse
        { 
            WalletTrackingId = walletTrackingId,
        };

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
                    ReturnUrl = request.ReturnUrl,
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
                    ReturnUrl = request.ReturnUrl,
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
                bool isCodeExist = true;
                int maxNumber = 999999;
                int tryCode = 1;
                do
                {
                    isCodeExist = await context.WalletTrackings.AnyAsync(wt => wt.PaymentCode == payOSPaymentCode.ToString());
                    tryCode++;
                } while (isCodeExist && tryCode <= maxNumber);
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

        try
        {
            List<string> deviceTokens = currentUser!.Devices.Select(d => d.Token).ToList();
            if (deviceTokens.Count > 0)
            {
                await fcmNotificationService.SendMultibleNotificationAsync(
                    deviceTokens,
                    "Nạp ví TechGadget",
                    "Bạn vừa tạo giao dịch nạp tiền vào ví TechGadget. Vui lòng thanh toán trước thời hạn.", 
                    new Dictionary<string, string>()
                    {
                        { "walletTrackingId", walletTrackingId.ToString() },
                    }
                );
            }
            await context.Notifications.AddAsync(new Notification
            {
                UserId = currentUser!.Id,
                Title = "Nạp ví TechGadget",
                Content = "Bạn vừa tạo giao dịch nạp tiền vào ví TechGadget. Vui lòng thanh toán trước thời hạn.",
                CreatedAt = createdAt,
                IsRead = false,
                Type = NotificationType.WalletTracking
            });
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return Ok(depositResponse);
    }

    private static long GenerateDailyRandomLong()
    {
        // Lấy ngày hiện tại (chỉ lấy phần ngày)
        DateTime today = DateTime.Today;

        // Tạo một seed từ ngày hôm nay bằng cách sử dụng tổng các thành phần ngày
        int seed = today.Year * 10000 + today.Month * 100 + today.Day;

        // Sử dụng seed để tạo Random (mỗi ngày sẽ có seed khác nhau)
        Random dayRandom = new Random(seed);

        // Lấy một số ngẫu nhiên từ seed theo ngày để tạo thành seed cho Random mới
        int randomSeed = dayRandom.Next();

        // Sử dụng DateTime.Now.Ticks để kết hợp thêm tính ngẫu nhiên
        Random random = new Random(randomSeed + (int)(DateTime.Now.Ticks % int.MaxValue));

        // Tạo số ngẫu nhiên từ 100000 đến 999999
        long randomLong = random.Next(100000, 1000000);

        return randomLong;
    }
}
