using WebApi.Services.Background.OrderDetails;
using WebApi.Services.Background.SellerApplications;
using WebApi.Services.Background.UserVerifies;
using WebApi.Services.Background.WalletTrackings;

namespace WebApi.Extensions;

public static class BackgroundServicesExtensions
{
    public static void AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<UserVerifyStatusCheckService>();
        services.AddHostedService<UserVerifyCleanupService>();
        services.AddHostedService<RejectSellerApplicationService>();
        services.AddHostedService<ExpiredTransactionService>();
        services.AddHostedService<SellerOrderService>();
    }
}
