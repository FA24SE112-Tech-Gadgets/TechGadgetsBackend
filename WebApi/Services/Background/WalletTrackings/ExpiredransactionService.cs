using WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Background.WalletTrackings;

public class ExpiredransactionService(IServiceProvider serviceProvider) : BackgroundService
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
                    .Where(a => a.CreatedAt <= DateTime.UtcNow.AddMinutes(-5))
                    .ExecuteUpdateAsync(sa => sa
                        .SetProperty(x => x.Status, Data.Entities.WalletTrackingStatus.Expired),
                        stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
