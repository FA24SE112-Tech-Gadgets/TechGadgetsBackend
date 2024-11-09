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
[RolesFilter(Role.Seller)]
public class ConfirmSellerOrder : ControllerBase
{
    [HttpPut("seller-order/{sellerOrderId}/confirm")]
    [Tags("Seller Orders")]
    [SwaggerOperation(
        Summary = "Confirm Seller Order By SellerOrderId",
        Description = "API is for cancel selelr order by sellerOrderId." +
                            "<br>&nbsp; - Seller chỉ được confirm selelrOrder mà liên quan đến bản thân" +
                            "<br>&nbsp; - Không thể confirm những selelrOrder đã Cancelled." +
                            "<br>&nbsp; - Sau khi sellerOrder confirm success thì hệ thống sẽ tự động sẽ tự động chuyển tiền vào ví Seller." +
                            "<br>&nbsp; - User bị Inactive thì không thể confirm đơn được."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid sellerOrderId, AppDbContext context, [FromServices] CurrentUserService currentUserService, [FromServices] FCMNotificationService fcmNotificationService)
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
            .Include(so => so.SellerOrderItems)
                    .ThenInclude(soi => soi.GadgetDiscount)
            .Include(so => so.Order)
                .ThenInclude(o => o.Customer)
                .ThenInclude(c => c.User)
                .ThenInclude(u => u.Devices)
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

        if (sellerOrder!.SellerId != currentUser!.Seller!.Id)
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

        sellerOrder!.Status = SellerOrderStatus.Success;

        var userWallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == currentUser!.Id);

        int totalAmount = 0;
        DateTime createdAt = DateTime.UtcNow;
        WalletTracking walletTracking = new WalletTracking()
        {
            WalletId = userWallet!.Id,
            SellerOrderId = sellerOrderId,
            Type = WalletTrackingType.SellerTransfer,
            Status = WalletTrackingStatus.Pending,
            CreatedAt = createdAt,
        }!;

        //Tính tổng cho Wallettracking
        var selelrOrderItems = sellerOrder.SellerOrderItems;
        foreach (var soi in selelrOrderItems)
        {
            int discountPercentage = soi.GadgetDiscount != null ? soi.GadgetDiscount.DiscountPercentage : 0;
            totalAmount += soi.GadgetQuantity * (int)Math.Ceiling(soi.GadgetPrice * (1 - discountPercentage / 100.0));
        }

        walletTracking.Amount = totalAmount;

        await context.WalletTrackings.AddAsync(walletTracking);
        await context.SaveChangesAsync();

        try
        {
            string customerTitle = $"Đơn hàng {sellerOrderId} đã xác nhận thành công";
            string customerContent = $"Đơn hàng {sellerOrderId} đã xác nhận thành công. Bạn có thể đánh giá các sản phẩm trong đơn hàng";
            string sellerTitle = $"Đơn hàng {sellerOrderId} đã xác nhận thành công";
            string sellerContent = $"Đơn hàng {sellerOrderId} đã xác nhận thành công. Hệ thống sẽ tiến hành chuyền tiền vào ví của bạn.";

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
                Type = NotificationType.SellerOrder
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
                Type = NotificationType.SellerOrder
            });
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return Ok("Xác nhận thành công");
    }
}
