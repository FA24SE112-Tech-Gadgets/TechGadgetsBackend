using WebApi.Common.Exceptions;

namespace WebApi.Extensions;

public static class ExceptionHandlerExtensions
{
    public static void UseTechGadgetsExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<TechGadgetExceptionHandler>();
    }
}
