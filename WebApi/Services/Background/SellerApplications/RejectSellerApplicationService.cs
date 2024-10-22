using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Services.Background.SellerApplications;

public class RejectSellerApplicationService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await context.SellerApplications
                    .Where(sa => sa.Status == Data.Entities.SellerApplicationStatus.Pending && sa.CreatedAt <= DateTime.UtcNow.AddDays(-3))
                    .ExecuteUpdateAsync(sa => sa
                        .SetProperty(x => x.Status, Data.Entities.SellerApplicationStatus.Rejected)
                        .SetProperty(x => x.RejectReason, "Đơn của bạn đã quá thời hạn xử lí. Xin vui lòng gửi lại đơn khác."),
                        stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
