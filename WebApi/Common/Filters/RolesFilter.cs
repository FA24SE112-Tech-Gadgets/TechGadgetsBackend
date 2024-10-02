using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Common.Exceptions;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Common.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class RolesFilterAttribute(params Role[] acceptedRoles) : Attribute, IAsyncActionFilter
{

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var currentUserService = context.HttpContext.RequestServices.GetRequiredService<CurrentUserService>();
        var user = await currentUserService.GetCurrentUser();

        if (user != null && acceptedRoles.Contains(user.Role))
        {
            await next();
        }
        else
        {
            var reason = new Reason("role", "Tài khoản không đủ thẩm quyền để truy cập API này.");
            var reasons = new List<Reason> { reason };
            var errorResponse = new TechGadgetErrorResponse
            {
                Code = TechGadgetErrorCode.WEA_01.Code,
                Title = TechGadgetErrorCode.WEA_01.Title,
                Reasons = reasons
            };

            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = (int)TechGadgetErrorCode.WEA_01.Status
            };
        }
    }
}