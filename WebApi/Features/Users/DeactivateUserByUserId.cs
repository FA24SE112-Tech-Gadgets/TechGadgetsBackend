using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;
using WebApi.Services.Notifications;

namespace WebApi.Features.Users;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Admin)]
public class DeactivateUserByUserId : ControllerBase
{
    [HttpPut("user/{userId}/deactivate")]
    [Tags("Users")]
    [SwaggerOperation(
        Summary = "Deactivate User",
        Description = "API for Admin to deactivate user. Note:" +
                            "<br>&nbsp; - User bị Inactive thì không thể deactive user khác được." +
                            "<br>&nbsp; - Reason nểu không truyền thì lấy default: 'Người dùng đã vi phạm chính sách của TechGadget.'"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid userId, AppDbContext context, [FromServices] CurrentUserService currentUserService, [FromServices] FCMNotificationService fcmNotificationService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        if (currentUser.Id == userId)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("admin", "Không thể tự khóa tài khoản của bản thân")
            .Build();
        }

        var user = await context.Users
            .Include(u => u.Seller)
                .ThenInclude(s => s.SellerOrders)
            .Include(u => u.Devices)
            .FirstOrDefaultAsync(g => g.Id == userId);
        if (user is null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("user", "Người dùng không tồn tại")
            .Build();
        }

        if (user.Role == Role.Admin)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("admin", "Không thể khóa tài khoản Admin")
            .Build();
        }

        if (user.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("user", $"Người dùng {userId} đã bị vô hiệu hoá từ trước")
            .Build();
        }

        if (user.Status == UserStatus.Pending)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("user", $"Không thể khóa tài khoản chưa được kích hoạt")
            .Build();
        }

        DateTime createdAt = DateTime.UtcNow;
        if (user.Role == Role.Seller)
        {
            try
            {
                string sellerTitle = $"Tài khoản của bạn đã bị khóa";
                string sellerContent = $"Tài khoản của bạn đã bị khóa do người bán vi phạm chính sách của TechGadget. Các đơn hàng liên quan sẽ bị hủy và hoàn tiền cho người mua.";

                //Tạo thông báo cho seller
                List<string> deviceTokens = user.Devices.Select(d => d.Token).ToList();
                if (deviceTokens.Count > 0)
                {
                    await fcmNotificationService.SendMultibleNotificationAsync(
                        deviceTokens,
                        sellerTitle,
                        sellerContent,
                        new Dictionary<string, string>()
                        {
                            { "userId", userId.ToString() },
                        }
                    );
                }
                await context.Notifications.AddAsync(new Notification
                {
                    UserId = userId,
                    Title = sellerTitle,
                    Content = sellerContent,
                    CreatedAt = createdAt,
                    IsRead = false,
                    Type = NotificationType.User
                });

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var sellerOrders = user.Seller!.SellerOrders.Where(so => so.Status == SellerOrderStatus.Pending).ToList();
            foreach (var sellerOrder in sellerOrders)
            {
                var sellerOrderDetail = await context.SellerOrders
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
                    .FirstOrDefaultAsync(so => so.Id == sellerOrder.Id);

                sellerOrderDetail!.Status = SellerOrderStatus.Cancelled;

                var customerWallet = await context.Wallets.FirstOrDefaultAsync(w => w.Id == sellerOrderDetail.Order.WalletTracking.WalletId);

                int totalAmount = 0;

                WalletTracking walletTracking = new WalletTracking()
                {
                    WalletId = customerWallet!.Id,
                    SellerOrderId = sellerOrder.Id,
                    Reason = "Đơn hàng đã được hoàn tiền về cho quý khách vì người bán đã vi phạm chính sách của TechGadget. ",
                    Type = WalletTrackingType.Refund,
                    Status = WalletTrackingStatus.Pending,
                    CreatedAt = createdAt,
                }!;

                //Hoàn lại quantity cho gadget của Seller và tính tổng cho Wallettracking
                var selelrOrderItems = sellerOrderDetail.SellerOrderItems;
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
                    string customerTitle = $"Đơn hàng {sellerOrder.Id} đã bị hủy";
                    string customerContent = $"Đơn hàng {sellerOrder.Id} đã bị hủy do người bán vi phạm chính sách của TechGadget";

                    //Tạo thông báo cho customer
                    List<string> deviceTokens = sellerOrderDetail.Order.Customer.User!.Devices.Select(d => d.Token).ToList();
                    if (deviceTokens.Count > 0)
                    {
                        await fcmNotificationService.SendMultibleNotificationAsync(
                            deviceTokens,
                            customerTitle,
                            customerContent,
                            new Dictionary<string, string>()
                            {
                                { "sellerOrderId", sellerOrder.Id.ToString() },
                            }
                        );
                    }
                    await context.Notifications.AddAsync(new Notification
                    {
                        UserId = sellerOrderDetail.Order.Customer.User!.Id,
                        Title = customerTitle,
                        Content = customerContent,
                        CreatedAt = createdAt,
                        IsRead = false,
                        Type = NotificationType.SellerOrder,
                        SellerOrderId = sellerOrder.Id
                    });

                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        else
        {
            try
            {
                string userTitle = $"Tài khoản của bạn đã bị khóa";
                string userContent = $"Tài khoản của bạn đã bị khóa do vi phạm chính sách của TechGadget.";

                //Tạo thông báo cho seller
                List<string> deviceTokens = user.Devices.Select(d => d.Token).ToList();
                if (deviceTokens.Count > 0)
                {
                    await fcmNotificationService.SendMultibleNotificationAsync(
                        deviceTokens,
                        userTitle,
                        userContent,
                        new Dictionary<string, string>()
                        {
                            { "userId", userId.ToString() },
                        }
                    );
                }
                await context.Notifications.AddAsync(new Notification
                {
                    UserId = userId,
                    Title = userTitle,
                    Content = userContent,
                    CreatedAt = createdAt,
                    IsRead = false,
                    Type = NotificationType.User
                });

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        user.Status = UserStatus.Inactive;

        await context.SaveChangesAsync();

        return Ok("Khóa người dùng thành công");
    }
}
