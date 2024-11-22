using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;
using WebApi.Services.Notifications;

namespace WebApi.Features.SellerOrders;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
[RequestValidation<Request>]
public class CancelSellerOrder : ControllerBase
{
    public new class Request
    {
        public string? Reason { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Reason)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Lý do không được để trống.")
                .NotEmpty().WithMessage("Lý do không được để trống.");
        }
    }

    [HttpPut("seller-order/{sellerOrderId}/cancel")]
    [Tags("Seller Orders")]
    [SwaggerOperation(
        Summary = "Cancel Seller Order By SellerOrderId",
        Description = "API is for cancel seller order by sellerOrderId." +
                            "<br>&nbsp; - Customer chỉ được cancel sellerOrder trước khi Seller confirm" +
                            "<br>&nbsp; - Seller và Customer không thể cancel sellerOrder khi đơn đã hoàn thành." +
                            "<br>&nbsp; - Sau khi sellerOrder canceled success thì hệ thống sẽ tự động hoàn tiền sau 1 phút." +
                            "<br>&nbsp; - User bị Inactive thì không cancel seller order được, hệ thống tự cancel và hoàn tiền cho (Nếu là customer, còn seller thì không hoàn tiền)."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromRoute] Guid sellerOrderId, AppDbContext context, [FromServices] CurrentUserService currentUserService, [FromServices] FCMNotificationService fcmNotificationService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        var sellerOrder = await context.SellerOrders
            .Include(so => so.Order)
                .ThenInclude(o => o.WalletTracking)
            .Include(so => so.Order)
                .ThenInclude(o => o.Customer)
                .ThenInclude(c => c.User)
                .ThenInclude(u => u.Devices)
            .Include(so => so.SellerOrderItems)
                .ThenInclude(gi => gi.Gadget)
            .Include(so => so.SellerOrderItems)
                .ThenInclude(soi => soi.GadgetDiscount)
            .Include(so => so.Seller)
                .ThenInclude(s => s.User)
                .ThenInclude(u => u.Devices)
            .FirstOrDefaultAsync(so => so.Id == sellerOrderId);
        if (sellerOrder == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerOrder", "Không tìm thấy đơn này.")
            .Build();
        }

        if ((currentUser!.Role == Role.Customer && sellerOrder!.Order.CustomerId != currentUser!.Customer!.Id) || (currentUser!.Role == Role.Seller && sellerOrder!.SellerId != currentUser!.Seller!.Id))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("sellerOrder", "Người dùng không đủ thẩm quyền để truy cập đơn này.")
            .Build();
        }

        if (sellerOrder.Status != SellerOrderStatus.Pending)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerOrder", "Đơn này đã Success/Cancelled rồi.")
            .Build();
        }

        sellerOrder!.Status = SellerOrderStatus.Cancelled;
        sellerOrder.UpdatedAt = DateTime.UtcNow;

        var customerWallet = await context.Wallets.FirstOrDefaultAsync(w => w.Id == sellerOrder.Order.WalletTracking.WalletId);

        int totalAmount = 0;
        DateTime createdAt = DateTime.UtcNow;
        WalletTracking walletTracking = new WalletTracking()
        {
            WalletId = customerWallet!.Id,
            SellerOrderId = sellerOrderId,
            Type = WalletTrackingType.Refund,
            Status = WalletTrackingStatus.Pending,
            CreatedAt = createdAt,
        }!;

        if (currentUser!.Role == Role.Customer)
        {
            walletTracking.Reason = "Khách hàng yêu cầu hủy đơn";
        }
        else
        {
            if (request.Reason == null)
            {
                throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("sellerOrder", "Lý do không được để trống.")
                .Build();
            }
            walletTracking.Reason = request.Reason;
        }


        //Hoàn lại quantity cho gadget của Seller và tính tổng cho Wallettracking
        var selelrOrderItems = sellerOrder.SellerOrderItems;
        foreach (var soi in selelrOrderItems)
        {
            int discountPercentage = soi.GadgetDiscount != null ? soi.GadgetDiscount.DiscountPercentage : 0;
            totalAmount += soi.GadgetQuantity * (int)Math.Ceiling(soi.GadgetPrice * (1 - discountPercentage / 100.0));
            soi.Gadget.Quantity += soi.GadgetQuantity;
        }

        walletTracking.Amount = totalAmount;
        await context.WalletTrackings.AddAsync(walletTracking);
        await context.SaveChangesAsync();

        try
        {
            string customerTitle = currentUser!.Role == Role.Customer ? $"Hủy đơn hàng {sellerOrderId} thành công" : $"Đơn hàng {sellerOrderId} đã bị hủy";
            string customerContent = currentUser!.Role == Role.Customer ? $"Bạn vừa hủy đơn hàng {sellerOrderId} thành công." : $"Đơn hàng {sellerOrderId} đã bị cửa hàng từ chối tiếp nhận";
            string sellerTitle = currentUser!.Role != Role.Customer ? $"Từ chối đơn hàng {sellerOrderId} thành công" : $"Đơn hàng {sellerOrderId} đã bị hủy";
            string sellerContent = currentUser!.Role != Role.Customer ? $"Bạn đã từ chối đơn hàng {sellerOrderId} thành công" : $"Khách hàng không muốn mua dơn hàng {sellerOrderId} này nữa";

            if (currentUser!.Role == Role.Customer)
            {
                //Tạo thông báo cho customer
                List<string> deviceTokens = currentUser!.Devices.Select(d => d.Token).ToList();
                if (deviceTokens.Count > 0)
                {
                    await fcmNotificationService.SendMultibleNotificationAsync(
                        deviceTokens,
                        customerTitle,
                        customerContent,
                        new Dictionary<string, string>()
                        {
                            { "sellerOrderId", sellerOrderId.ToString() },
                            { "notiType", NotificationType.SellerOrder.ToString() },
                            { "userId", currentUser.Id.ToString() } ,
                        }
                    );
                }
                await context.Notifications.AddAsync(new Notification
                {
                    UserId = currentUser!.Id,
                    Title = customerTitle,
                    Content = customerContent,
                    CreatedAt = createdAt,
                    IsRead = false,
                    Type = NotificationType.SellerOrder,
                    SellerOrderId = sellerOrderId,
                });

                //Tạo thông báo cho seller
                deviceTokens = sellerOrder.Seller.User.Devices.Select(d => d.Token).ToList();
                if (deviceTokens.Count > 0)
                {
                    await fcmNotificationService.SendMultibleNotificationAsync(
                        deviceTokens,
                        sellerTitle,
                        sellerContent,
                        new Dictionary<string, string>()
                        {
                            { "sellerOrderId", sellerOrderId.ToString() },
                            { "notiType", NotificationType.SellerOrder.ToString() },
                            { "userId", sellerOrder.Seller.User.Id.ToString() },
                        }
                    );
                }
                await context.Notifications.AddAsync(new Notification
                {
                    UserId = sellerOrder.Seller.User.Id,
                    Title = sellerTitle,
                    Content = sellerContent,
                    CreatedAt = createdAt,
                    IsRead = false,
                    Type = NotificationType.SellerOrder,
                    SellerOrderId = sellerOrderId
                });
            }
            else
            {
                //Tạo thông báo cho seller
                List<string> deviceTokens = currentUser!.Devices.Select(d => d.Token).ToList();
                if (deviceTokens.Count > 0)
                {
                    await fcmNotificationService.SendMultibleNotificationAsync(
                        deviceTokens,
                        sellerTitle,
                        sellerContent,
                        new Dictionary<string, string>()
                        {
                            { "sellerOrderId", sellerOrderId.ToString() },
                            { "notiType", NotificationType.SellerOrder.ToString() },
                            { "userId", currentUser.Id.ToString() } ,
                        }
                    );
                }
                await context.Notifications.AddAsync(new Notification
                {
                    UserId = currentUser!.Id,
                    Title = sellerTitle,
                    Content = sellerContent,
                    CreatedAt = createdAt,
                    IsRead = false,
                    Type = NotificationType.SellerOrder,
                    SellerOrderId = sellerOrderId
                });

                //Tạo thông báo cho customer
                deviceTokens = sellerOrder.Order.Customer.User.Devices.Select(d => d.Token).ToList();
                if (deviceTokens.Count > 0)
                {
                    await fcmNotificationService.SendMultibleNotificationAsync(
                        deviceTokens,
                        customerTitle,
                        customerContent,
                        new Dictionary<string, string>()
                        {
                            { "sellerOrderId", sellerOrderId.ToString() },
                            { "notiType", NotificationType.SellerOrder.ToString() },
                            { "userId", sellerOrder.Order.Customer.User.Id.ToString() } ,
                        }
                    );
                }
                await context.Notifications.AddAsync(new Notification
                {
                    UserId = sellerOrder.Order.Customer.User.Id,
                    Title = customerTitle,
                    Content = customerContent,
                    CreatedAt = createdAt,
                    IsRead = false,
                    Type = NotificationType.SellerOrder,
                    SellerOrderId = sellerOrderId
                });
            }
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return Ok("Hủy thành công");
    }
}
