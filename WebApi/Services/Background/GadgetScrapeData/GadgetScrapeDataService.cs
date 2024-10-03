using WebApi.Data.Entities;
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
                Console.WriteLine(await scrapeTGDDDataService.ScrapeGadgetByBrand(
                    "https://www.thegioididong.com/dtdd#c=42&m=2283&o=13&pi=0",
                    "Điện thoại",
                    "Samsung"
                    ));
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
