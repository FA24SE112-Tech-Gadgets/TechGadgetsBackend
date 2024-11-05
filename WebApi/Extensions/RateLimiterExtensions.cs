using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using WebApi.Common.Exceptions;

namespace WebApi.Extensions;

public static class RateLimiterExtensions
{
    public static void AddTechGadgetRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOptions.AddConcurrencyLimiter("concurrency", options =>
            {
                options.PermitLimit = 20;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 10;
            });
        });
    }

    public static void UseTechGadgetRateLimiter(this IApplicationBuilder app)
    {
        app.UseMiddleware<RateLimitExceptionMiddleware>();
        app.UseRateLimiter();
    }
}

public class RateLimitExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        await next(httpContext);

        if (httpContext.Response.StatusCode == StatusCodes.Status429TooManyRequests)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEA_02)
                .AddReason("rateLimit", "Lỗi vượt quá request cho phép")
                .Build();
        }
    }
}
