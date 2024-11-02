using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Common.Exceptions;
using WebApi.Common.Settings;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth.Models;

namespace WebApi.Services.Auth;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSettings, AppDbContext context)
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(jwtSettings.Value.SigningKey));

    public async Task<User?> GetCurrentUser()
    {
        var request = httpContextAccessor.HttpContext?.Request;
        var authHeader = request?.Headers.Authorization.ToString();
        var token = authHeader?.Replace("Bearer ", string.Empty);

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = _key,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            ClockSkew = TimeSpan.Zero
        };

        if (token == "")
        {
            return null;
        }

        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

        var userInfoJson = principal.Claims.FirstOrDefault(c => c.Type == "UserInfo")?.Value;

        var userInfo = JsonConvert.DeserializeObject<TokenRequest>(userInfoJson!);

        return await context.Users
            .Include(u => u.Manager)
            .Include(u => u.Admin)
            .Include(u => u.Customer)
            .Include(u => u.Seller)
            .Include(u => u.Wallet)
            .Include(u => u.Devices)
            .FirstOrDefaultAsync(x => x.Id == userInfo!.Id);
    }

    public async Task<Guid> GetCurrentUserId()
    {
        var user = await GetCurrentUser();

        if (user == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("user", "Người dùng không tồn tại.")
                .Build();
        }

        return user.Id;
    }
}
