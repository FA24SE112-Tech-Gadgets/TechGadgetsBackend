using WebApi.Data.Entities;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;

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

                //Refund and Confirm SellerOrder Case:
                var walletTrackings = await context.WalletTrackings
                    .Include(wt => wt.Wallet)
                    .Where(wt => (wt.Type == WalletTrackingType.Refund && wt.Status == WalletTrackingStatus.Pending)
                        || (wt.Type == WalletTrackingType.SellerTransfer && wt.Status == WalletTrackingStatus.Pending))
                    .ToListAsync(stoppingToken);

                var systemWallet = await context.SystemWallets.SingleOrDefaultAsync(stoppingToken);

                foreach (var wt in walletTrackings)
                {
                    var systemSellerOrderTracking = await context.SystemSellerOrderTrackings
                        .FirstOrDefaultAsync(ssot => ssot.SellerOrderId == wt.SellerOrderId, stoppingToken);

                    var userWallet = wt.Wallet;
                    if (wt.Type == WalletTrackingType.Refund)
                    {
                        //Trừ tiền trong system, hoàn về cho customer
                        systemWallet!.Amount -= wt.Amount;
                        userWallet!.Amount += wt.Amount;

                        //Update SystemSellerOrderTrackingStatus = Refunded
                        systemSellerOrderTracking!.Status = SystemSellerOrderTrackingStatus.Refunded;
                        systemSellerOrderTracking!.UpdatedAt = DateTime.UtcNow;

                        //Update WalletTracking của Customer
                        wt.Status = WalletTrackingStatus.Success;
                        wt.RefundedAt = DateTime.UtcNow;
                    }
                    if (wt.Type == WalletTrackingType.SellerTransfer)
                    {
                        //Trừ tiền trong system, chuyển cho seller
                        systemWallet!.Amount -= wt.Amount;
                        userWallet!.Amount += wt.Amount;

                        //Update SystemSellerOrderTrackingStatus = Paid
                        systemSellerOrderTracking!.Status = SystemSellerOrderTrackingStatus.Paid;
                        systemSellerOrderTracking!.UpdatedAt = DateTime.UtcNow;

                        //Update WalletTracking của Seller
                        wt.Status = WalletTrackingStatus.Success;
                        wt.SellerPaidAt = DateTime.UtcNow;
                    }
                    await context.SaveChangesAsync(stoppingToken);
                }

                //Time Confirm Expired Case
                var systemSellerOrderTrackings = await context.SystemSellerOrderTrackings
                    .Include(ssot => ssot.FromUser)
                        .ThenInclude(u => u.Wallet)
                    .Include(ssot => ssot.SellerOrder)
                        .ThenInclude(so => so.SellerOrderItems)
                            .ThenInclude(soi => soi.Gadget)
                    .Where(ssot => ssot.CreatedAt <= DateTime.UtcNow.AddMinutes(-5) && ssot.Status == SystemSellerOrderTrackingStatus.Pending)
                    .ToListAsync(stoppingToken);

                foreach (var ssot in systemSellerOrderTrackings)
                {
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
                    ssot!.UpdatedAt = DateTime.UtcNow;

                    //Create WalletTracking của Customer
                    WalletTracking walletTracking = new WalletTracking()
                    {
                        WalletId = userWallet!.Id,
                        SellerOrderId = ssot.SellerOrderId,
                        Amount = totalAmount,
                        Type = WalletTrackingType.Refund,
                        Status = WalletTrackingStatus.Success,
                        CreatedAt = DateTime.UtcNow,
                        RefundedAt = DateTime.UtcNow,
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
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
