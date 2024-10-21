using WebApi.Data;
using Microsoft.EntityFrameworkCore;

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

                await context.WalletTrackings
                    .Where(wt => wt.CreatedAt <= DateTime.UtcNow.AddMinutes(-5) && wt.Status == Data.Entities.WalletTrackingStatus.Pending)
                    .ExecuteUpdateAsync(wt => wt
                        .SetProperty(x => x.Status, Data.Entities.WalletTrackingStatus.Expired),
                        stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
