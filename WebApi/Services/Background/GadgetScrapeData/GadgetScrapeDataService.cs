using System.Security.Authentication.ExtendedProtection;
using WebApi.Data;
using WebApi.Services.ScrapeData;

namespace WebApi.Services.Background.GadgetScrapeData;

public class GadgetScrapeDataService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var scrapeTGDDDataService = scope.ServiceProvider.GetRequiredService<ScrapeTGDDDataService>();
                var scrapeFPTShopDataService = scope.ServiceProvider.GetRequiredService<ScrapeFPTShopDataService>();
                //await scrapeTGDDDataService.ScrapeTGDDGadget();
                await scrapeFPTShopDataService.ScrapeGadgetByBrand("https://fptshop.com.vn/dien-thoai/samsung");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
