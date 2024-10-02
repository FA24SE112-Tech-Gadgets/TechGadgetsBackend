using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Data.Entities;

namespace WebApi.Services.Background.UserVerifies;

public class UserVerifyCleanupService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await context.UserVerifies
                    .Where(a => a.VerifyStatus == VerifyStatus.Expired)
                    .ExecuteDeleteAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(60 * 30), stoppingToken);
        }
    }
}

