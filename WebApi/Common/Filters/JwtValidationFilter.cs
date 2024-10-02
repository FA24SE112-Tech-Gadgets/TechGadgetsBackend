using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Common.Exceptions;
using WebApi.Common.Settings;

namespace WebApi.Common.Filters;

[AttributeUsage(AttributeTargets.All)]
public class JwtValidationAttribute : Attribute, IAsyncAuthorizationFilter
{
    private const string AuthorizationHeader = "Authorization";
    private const string BearerPrefix = "Bearer ";

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var jwtSettings = context.HttpContext.RequestServices.GetRequiredService<IOptions<JwtSettings>>().Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey));

        if (!context.HttpContext.Request.Headers.TryGetValue(AuthorizationHeader, out var authHeader) ||
            !authHeader.ToString().StartsWith(BearerPrefix))
        {
            context.Result = CreateErrorResult(TechGadgetErrorCode.WEA_00, "Thiếu mã Token");
            return Task.CompletedTask;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        string token;
        try
        {
            token = context.HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        }
        catch (Exception)
        {
            context.Result = CreateErrorResult(TechGadgetErrorCode.WEB_02, "Mã Token không hợp lệ.");
            return Task.CompletedTask;
        }

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            var userInfoJson = principal.Claims.FirstOrDefault(c => c.Type == "UserInfo")?.Value;
            if (string.IsNullOrEmpty(userInfoJson))
            {
                context.Result = CreateErrorResult(TechGadgetErrorCode.WEA_00, "Không có thông tin người dùng trong mã Token.");
                return Task.CompletedTask;
            }

            var checkClaim = principal.Claims.FirstOrDefault(c => c.Type == "TokenClaim" && c.Value == "ForVerifyOnly")?.Value;
            if (string.IsNullOrEmpty(checkClaim))
            {
                context.Result = CreateErrorResult(TechGadgetErrorCode.WEA_00, "Thiếu thông tin xác thực trong mã Token.");
                return Task.CompletedTask;
            }

            context.HttpContext.User = principal;
        }
        catch (SecurityTokenException)
        {
            context.Result = CreateErrorResult(TechGadgetErrorCode.WEB_02, "Mã Token không hợp lệ hoặc đã hết hạn.");
        }

        return Task.CompletedTask;
    }

    private static IActionResult CreateErrorResult(TechGadgetErrorCode errorCode, string message)
    {
        var reason = new Reason("token", message);
        var reasons = new List<Reason> { reason };
        var errorResponse = new TechGadgetErrorResponse
        {
            Code = errorCode.Code,
            Title = errorCode.Title,
            Reasons = reasons
        };
        return new JsonResult(errorResponse) { StatusCode = (int)errorCode.Status };
    }
}