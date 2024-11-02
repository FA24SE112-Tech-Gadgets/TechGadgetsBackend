using WebApi.Data.Entities;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.Services.Notifications;

namespace WebApi.Services.Background.OrderDetails;

public class SellerOrderService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var fcmNotificationService = scope.ServiceProvider.GetRequiredService<FCMNotificationService>();

                //Refund and Confirm SellerOrder Case:
                var walletTrackings = await context.WalletTrackings
                    .Include(wt => wt.Wallet)
                        .ThenInclude(w => w.User)
                        .ThenInclude(u => u.Devices)
                    .Where(wt => (wt.Type == WalletTrackingType.Refund && wt.Status == WalletTrackingStatus.Pending)
                        || (wt.Type == WalletTrackingType.SellerTransfer && wt.Status == WalletTrackingStatus.Pending))
                    .ToListAsync(stoppingToken);

                var systemWallet = await context.SystemWallets.SingleOrDefaultAsync(stoppingToken);

                foreach (var wt in walletTrackings)
                {
                    var systemSellerOrderTracking = await context.SystemSellerOrderTrackings
                        .FirstOrDefaultAsync(ssot => ssot.SellerOrderId == wt.SellerOrderId, stoppingToken);

                    var userWallet = wt.Wallet;
                    DateTime currentTime = DateTime.UtcNow;
                    if (wt.Type == WalletTrackingType.Refund)
                    {
                        //Trừ tiền trong system, hoàn về cho customer
                        systemWallet!.Amount -= wt.Amount;
                        userWallet!.Amount += wt.Amount;

                        //Update SystemSellerOrderTrackingStatus = Refunded
                        systemSellerOrderTracking!.Status = SystemSellerOrderTrackingStatus.Refunded;
                        systemSellerOrderTracking!.UpdatedAt = currentTime;

                        //Update WalletTracking của Customer
                        wt.Status = WalletTrackingStatus.Success;
                        wt.RefundedAt = currentTime;
                    }
                    if (wt.Type == WalletTrackingType.SellerTransfer)
                    {
                        //Trừ tiền trong system, chuyển cho seller
                        systemWallet!.Amount -= wt.Amount;
                        userWallet!.Amount += wt.Amount;

                        //Update SystemSellerOrderTrackingStatus = Paid
                        systemSellerOrderTracking!.Status = SystemSellerOrderTrackingStatus.Paid;
                        systemSellerOrderTracking!.UpdatedAt = currentTime;

                        //Update WalletTracking của Seller
                        wt.Status = WalletTrackingStatus.Success;
                        wt.SellerPaidAt = currentTime;
                    }
                    await context.SaveChangesAsync(stoppingToken);

                    try
                    {
                        string customerTitle = $"Hoàn tiền đơn hàng {wt.SellerOrderId} thành công";
                        string customerContent = $"Đơn hàng {wt.SellerOrderId} đã được hoàn tiền thành công";
                        string sellerTitle = $"Tiền của đơn hàng {wt.SellerOrderId} đã về!";
                        string sellerContent = $"Tiền của đơn hàng {wt.SellerOrderId} đã về. Hãy truy cập vào ví để kiểm tra.";


                        List<string> deviceTokens = wt.Wallet.User!.Devices.Select(d => d.Token).ToList();
                        if (deviceTokens.Count > 0)
                        {
                            await fcmNotificationService.SendMultibleNotificationAsync(
                                deviceTokens,
                                wt.Type == WalletTrackingType.Refund ? customerTitle : sellerTitle,
                                wt.Type == WalletTrackingType.Refund ? customerContent : sellerContent,
                                new Dictionary<string, string>()
                                {
                                    { "walletTrackingId", wt.Id.ToString()! },
                                    { "sellerOrderId", wt.SellerOrderId.ToString()! },
                                }
                            );
                        }
                        if (wt.Type == WalletTrackingType.Refund)
                        {
                            //Tạo thông báo cho customer
                            await context.Notifications.AddAsync(new Data.Entities.Notification
                            {
                                UserId = wt.Wallet.UserId,
                                Title = customerTitle,
                                Content = customerContent,
                                CreatedAt = currentTime,
                                IsRead = false,
                                Type = NotificationType.WalletTracking
                            }, stoppingToken);
                        }
                        if (wt.Type == WalletTrackingType.SellerTransfer)
                        {
                            //Tạo thông báo cho seller
                            await context.Notifications.AddAsync(new Data.Entities.Notification
                            {
                                UserId = wt.Wallet.UserId,
                                Title = sellerTitle,
                                Content = sellerContent,
                                CreatedAt = currentTime,
                                IsRead = false,
                                Type = NotificationType.WalletTracking
                            }, stoppingToken);
                        }

                        await context.SaveChangesAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Time Confirm Expired Case
                var systemSellerOrderTrackings = await context.SystemSellerOrderTrackings
                    .Include(ssot => ssot.FromUser)
                        .ThenInclude(u => u.Wallet)
                    .Include(ssot => ssot.FromUser)
                        .ThenInclude(u => u.Devices)
                    .Include(ssot => ssot.ToUser)
                        .ThenInclude(u => u.Devices)
                    .Include(ssot => ssot.SellerOrder)
                        .ThenInclude(so => so.SellerOrderItems)
                            .ThenInclude(soi => soi.Gadget)
                    .Where(ssot => ssot.CreatedAt <= DateTime.UtcNow.AddMinutes(-5) && ssot.Status == SystemSellerOrderTrackingStatus.Pending)
                    .ToListAsync(stoppingToken);

                foreach (var ssot in systemSellerOrderTrackings)
                {
                    DateTime currentTime = DateTime.UtcNow;
                    var userWallet = ssot.FromUser.Wallet;

                    int totalAmount = 0;
                    foreach (var soi in ssot.SellerOrder.SellerOrderItems)
                    {
                        totalAmount += (soi.GadgetPrice * soi.GadgetQuantity);
                    }

                    //Trừ tiền trong system, hoàn về cho customer
                    systemWallet!.Amount -= totalAmount;
                    userWallet!.Amount += totalAmount;

                    //Update SystemSellerOrderTrackingStatus = Refunded
                    ssot!.Status = SystemSellerOrderTrackingStatus.Refunded;
                    ssot!.UpdatedAt = currentTime;

                    //Create WalletTracking của Customer
                    WalletTracking walletTracking = new WalletTracking()
                    {
                        WalletId = userWallet!.Id,
                        SellerOrderId = ssot.SellerOrderId,
                        Amount = totalAmount,
                        Type = WalletTrackingType.Refund,
                        Status = WalletTrackingStatus.Success,
                        CreatedAt = currentTime,
                        RefundedAt = currentTime,
                        Reason = "Đơn của bạn đã quá thời gian chờ xác nhận"
                    }!;

                    //Update OrderDetail status = Cancelled
                    ssot.SellerOrder.Status = SellerOrderStatus.Cancelled;

                    await context.WalletTrackings.AddAsync(walletTracking, stoppingToken);

                    //Hoàn lại quantity cho gadget của Seller
                    var sellerOrderItems = ssot.SellerOrder.SellerOrderItems;
                    foreach (var soi in sellerOrderItems)
                    {
                        soi.Gadget.Quantity += soi.GadgetQuantity;
                    }

                    await context.SaveChangesAsync(stoppingToken);

                    try
                    {
                        string customerTitle = $"Đơn hàng {ssot.SellerOrderId} đã quá thời gian xử lí";
                        string customerContent = $"Đơn hàng {ssot.SellerOrderId} đã quá thời gian xử lí. Hệ thống đã tiến hành hoàn tiền cho bạn.";
                        string sellerTitle = $"Đơn hàng {ssot.SellerOrderId} đã quá thời gian xác nhận";
                        string sellerContent = $"Đơn hàng {ssot.SellerOrderId} đã quá thời gian xác nhận. Bạn đã bỏ lỡ 1 đơn hàng rồi.";

                        //Tạo thông báo cho customer
                        List<string> deviceTokens = ssot.FromUser!.Devices.Select(d => d.Token).ToList();
                        if (deviceTokens.Count > 0)
                        {
                            await fcmNotificationService.SendMultibleNotificationAsync(
                                deviceTokens,
                                customerTitle,
                                customerContent,
                                new Dictionary<string, string>()
                                {
                                    { "sellerOrderId", ssot.SellerOrderId.ToString() },
                                }
                            );
                        }
                        await context.Notifications.AddAsync(new Data.Entities.Notification
                        {
                            UserId = ssot.FromUser!.Id,
                            Title = customerTitle,
                            Content = customerContent,
                            CreatedAt = currentTime,
                            IsRead = false,
                            Type = NotificationType.WalletTracking
                        }, stoppingToken);

                        //Tạo thông báo cho seller
                        deviceTokens = ssot.ToUser.Devices.Select(d => d.Token).ToList();
                        if (deviceTokens.Count > 0)
                        {
                            await fcmNotificationService.SendMultibleNotificationAsync(
                                deviceTokens,
                                sellerTitle,
                                sellerContent,
                                new Dictionary<string, string>()
                                {
                                    { "sellerOrderId", ssot.SellerOrderId.ToString() },
                                }
                            );
                        }
                        await context.Notifications.AddAsync(new Data.Entities.Notification
                        {
                            UserId = ssot.SellerOrder.Seller.User.Id,
                            Title = sellerTitle,
                            Content = sellerContent,
                            CreatedAt = currentTime,
                            IsRead = false,
                            Type = NotificationType.SellerOrder
                        }, stoppingToken);
                        await context.SaveChangesAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
