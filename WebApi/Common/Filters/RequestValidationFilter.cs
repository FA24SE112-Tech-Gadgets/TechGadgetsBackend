using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Common.Exceptions;

namespace WebApi.Common.Filters;

public class RequestValidationAttribute<TRequest> : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<TRequest>>();

        if (validator == null)
        {
            await next();
            return;
        }

        var request = context.ActionArguments.Values.OfType<TRequest>().FirstOrDefault();

        var validationResult = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);
        if (!validationResult.IsValid)
        {
            var errorResponse = new TechGadgetErrorResponse
            {
                Code = TechGadgetErrorCode.WEV_00.Code,
                Title = TechGadgetErrorCode.WEV_00.Title,
                Reasons = validationResult.Errors.Select(err => new Reason(err.PropertyName, err.ErrorMessage)).ToList()
            };
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = (int)TechGadgetErrorCode.WEV_00.Status
            };
            return;
        }

        await next();
    }
}