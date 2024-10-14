using WebApi.Services.Background.UserVerifies;

namespace WebApi.Extensions;

public static class BackgroundServicesExtensions
{
    public static void AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<UserVerifyStatusCheckService>();
        services.AddHostedService<UserVerifyCleanupService>();
    }
}
