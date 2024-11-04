using WebApi.Services.AI;
using WebApi.Services.Auth;
using WebApi.Services.Embedding;
using WebApi.Services.Mail;
using WebApi.Services.Notifications;
using WebApi.Services.Payment;
using WebApi.Services.Server;
using WebApi.Services.Storage;
using WebApi.Services.VerifyCode;

namespace WebApi.Extensions;

public static class ServicesExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<TokenService>();
        services.AddScoped<MailService>();
        services.AddScoped<VerifyCodeService>();
        services.AddScoped<CurrentUserService>();
        services.AddScoped<CurrentServerService>();
        services.AddScoped<GoogleStorageService>();
        services.AddScoped<VnPayPaymentService>();
        services.AddScoped<MomoPaymentService>();
        services.AddScoped<PayOSPaymentSerivce>();
        services.AddScoped<EmbeddingService>();
        services.AddScoped<FCMNotificationService>();
        services.AddScoped<NaturalLanguageService>();
    }
}
