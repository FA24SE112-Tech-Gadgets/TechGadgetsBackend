using WebApi.Data.Entities;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Background.OrderDetails;

public class OrderDetailService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var walletTrackings = await context.WalletTrackings
                    .Include(wt => wt.Wallet)
                    .Where(wt => (wt.Type == WalletTrackingType.Refund && wt.Status == WalletTrackingStatus.Pending)
                        || (wt.Type == WalletTrackingType.SellerTransfer && wt.Status == WalletTrackingStatus.Pending))
                    .ToListAsync(stoppingToken);

                foreach (var wt in walletTrackings)
                {
                    var systemOrderDetailTracking = await context.SystemOrderDetailTrackings
                        .FirstOrDefaultAsync(sodt => sodt.OrderDetailId == wt.OrderDetailId, stoppingToken);

                    var systemWallet = await context.SystemWallets.SingleOrDefaultAsync(stoppingToken);
                    var userWallet = wt.Wallet;
                    if (wt.Type == WalletTrackingType.Refund)
                    {
                        //Trừ tiền trong system, hoàn về cho customer
                        systemWallet!.Amount -= wt.Amount;
                        userWallet!.Amount += wt.Amount;

                        //Update SystemOrderDetailStatus = Refunded
                        systemOrderDetailTracking!.Status = SystemOrderDetailTrackingStatus.Refunded;
                        systemOrderDetailTracking!.UpdatedAt = DateTime.UtcNow;

                        //Update WalletTracking của Customer
                        wt.Status = WalletTrackingStatus.Success;
                        wt.RefundedAt = DateTime.UtcNow;
                    }
                    if (wt.Type == WalletTrackingType.SellerTransfer)
                    {
                        //Trừ tiền trong system, chuyển cho seller
                        systemWallet!.Amount -= wt.Amount;
                        userWallet!.Amount += wt.Amount;

                        //Update SystemOrderDetailStatus = Paid
                        systemOrderDetailTracking!.Status = SystemOrderDetailTrackingStatus.Paid;
                        systemOrderDetailTracking!.UpdatedAt = DateTime.UtcNow;

                        //Update WalletTracking của Seller
                        wt.Status = WalletTrackingStatus.Success;
                        //wt.PaidAt = DateTime.UtcNow;
                    }
                    await context.SaveChangesAsync(stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
