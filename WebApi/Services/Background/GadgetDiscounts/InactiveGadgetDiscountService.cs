using WebApi.Data.Entities;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Background.GadgetDiscounts;

public class InactiveGadgetDiscountService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var expiredGadgetDiscounts = await context.GadgetDiscounts
                    .Where(gd => gd.Status == GadgetDiscountStatus.Active && gd.ExpiredDate == DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                foreach (var ex in expiredGadgetDiscounts)
                {
                    ex.Status = GadgetDiscountStatus.Expired;
                    await context.SaveChangesAsync(stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
