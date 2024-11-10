using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Notifications;

namespace WebApi.Services.Background.WalletTrackings;

public class ExpiredTransactionService(IServiceProvider serviceProvider) : BackgroundService
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

                DateTime currentTime = DateTime.UtcNow;
                var walletTrackings = await context.WalletTrackings
                    .Include(wt => wt.Wallet)
                        .ThenInclude(w => w.User)
                            .ThenInclude(u => u.Devices)
                    .Where(wt => wt.CreatedAt <= currentTime.AddMinutes(-5) && wt.Status == WalletTrackingStatus.Pending)
                    .ToListAsync(stoppingToken);

                foreach (var walletTracking in walletTrackings)
                {
                    walletTracking.Status = WalletTrackingStatus.Expired;
                    await context.SaveChangesAsync(stoppingToken);

                    //Tạo thống báo cho customer
                    List<string> deviceTokens = walletTracking.Wallet.User!.Devices.Select(d => d.Token).ToList();
                    if (deviceTokens.Count > 0)
                    {
                        await fcmNotificationService.SendMultibleNotificationAsync(
                            deviceTokens,
                            "Đã hết thời gian thanh toán",
                            "Đã hết thời gian thanh toán. Vui lòng tạo mới 1 giao dịch khác để tiến hành nạp tiền.",
                            new Dictionary<string, string>()
                            {
                                { "walletTrackingId", walletTracking.Id.ToString()! },
                            }
                        );
                    }
                    await context.Notifications.AddAsync(new Data.Entities.Notification
                    {
                        UserId = walletTracking.Wallet.User!.Id,
                        Title = "Đã hết thời gian thanh toán",
                        Content = $"Đã hết thời gian thanh toán, mã giao dịch {walletTracking.Id}. Vui lòng tạo mới 1 giao dịch khác để tiến hành nạp tiền.",
                        CreatedAt = currentTime,
                        IsRead = false,
                        Type = NotificationType.WalletTracking
                    }, stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
