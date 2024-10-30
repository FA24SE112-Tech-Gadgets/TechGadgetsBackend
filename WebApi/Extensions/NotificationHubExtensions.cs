﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Common.Settings;
using WebApi.Features.Notifications;

namespace WebApi.Extensions;

public static class NotificationHubExtensions
{
    public static void UseNotificationHubHandler(this IEndpointRouteBuilder app)
    {
        app.MapHub<NotificationHub>("notification/hub");
    }

    public static void AddSignalRService(this IServiceCollection services)
    {
        services.AddSignalR();
    }

    public static void AddAuthenticationForSignalR(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JWT").Get<JwtSettings>();
        SymmetricSecurityKey key = new (Encoding.UTF8.GetBytes(jwtSettings!.SigningKey));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notification/hub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void AddAuthorizationForSignalR(this IServiceCollection services)
    {
        services.AddAuthorization();
    }
}