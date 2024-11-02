using Microsoft.IdentityModel.Tokens;

namespace WebApi.Common.Exceptions;

public class TechGadgetExceptionHandler(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ILogger<TechGadgetExceptionHandler> logger)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, context);
        }
    }

    private static async Task HandleExceptionAsync(Exception ex, HttpContext context)
    {
        if (ex is TechGadgetException techGadgetException)
        {
            var errorResponse = new TechGadgetErrorResponse
            {
                Code = techGadgetException.ErrorCode.Code,
                Title = techGadgetException.ErrorCode.Title,
                Reasons = techGadgetException.GetReasons().Select(reason => new Reason(reason.Title, reason.ReasonMessage)).ToList()
            };

            context.Response.StatusCode = (int)techGadgetException.ErrorCode.Status;
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        else if (ex is SecurityTokenException) // JWT-specific exception
        {
            var errorResponse = new TechGadgetErrorResponse
            {
                Code = TechGadgetErrorCode.WEB_03.Code,
                Title = TechGadgetErrorCode.WEB_03.Title,
                Reasons = new List<Reason>{
                    new Reason("token", "Mã Token không hợp lệ.")!
                }
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        else if (ex is UnauthorizedAccessException)
        {
            var errorResponse = new TechGadgetErrorResponse
            {
                Code = TechGadgetErrorCode.WEA_00.Code,
                Title = TechGadgetErrorCode.WEA_00.Title,
                Reasons = new List<Reason>{
                    new Reason("access", "Truy cập bị từ chối.")!
                }
            };
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        else
        {
            var errorResponse = new TechGadgetErrorResponse
            {
                Code = TechGadgetErrorCode.WES_00.Code,
                Title = TechGadgetErrorCode.WES_00.Title,
                Reasons = new List<Reason>{
                    new Reason("server", "Lỗi không mong muốn.")!
                }
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
