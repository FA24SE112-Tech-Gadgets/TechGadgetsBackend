﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http.Headers;
using WebApi.Common.Exceptions;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Auth.Mappers;
using WebApi.Features.Auth.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Auth;

[ApiController]
public class LoginGoogleController : ControllerBase
{
    public new record Request(string? DeviceToken);

    [HttpPost("auth/google/{accessToken}")]
    [Tags("Auth")]
    [SwaggerOperation(
        Summary = "Google login user",
        Description = "This API is for user login with Google. Note:" +
                            "<br>&nbsp; - User bị Inactive thì vẫn Login GG vô được (Vì liên quan đến tiền trong ví)."
    )]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromRoute] string accessToken, AppDbContext context, [FromServices] TokenService tokenService)
    {
        try
        {
            using (var client = new HttpClient())
            {
                // Set the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Make the GET request
                var response = await client.GetAsync($"https://www.googleapis.com/oauth2/v1/userinfo?access_token={accessToken}");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                var ggResponse = JsonConvert.DeserializeObject<LoginGoogleResponse>(responseContent);
                var user = await context.Users
                    .Where(a => a.Email == ggResponse!.Email)
                    .FirstOrDefaultAsync();

                //TH Mail chưa tồn tại
                if (user == null)
                {
                    var registerUserRequest = new RegisterUserRequest
                    {
                        Email = ggResponse!.Email,
                        Role = Role.Customer,
                        FullName = ggResponse.Name,
                        AvatarUrl = ggResponse.Picture,
                        LoginMethod = LoginMethod.Google,
                        Status = UserStatus.Active
                    };
                    if (!request.DeviceToken.IsNullOrEmpty())
                    {
                        registerUserRequest.Devices.Add(new Device
                        {
                            Token = request.DeviceToken!,
                        });
                    }
                    context.Users.Add(registerUserRequest.ToUserRequest()!);
                    await context.SaveChangesAsync();

                    user = await context.Users
                        .Include(u => u.Customer)
                        .Where(a => a.Email == ggResponse.Email)
                        .FirstOrDefaultAsync();

                    Wallet wallet = new Wallet
                    {
                        User = user!,
                        Amount = 0
                    }!;
                    Cart cart = new Cart
                    {
                        Customer = user!.Customer!,
                    }!;
                    await context.Wallets.AddAsync(wallet);
                    await context.Carts.AddAsync(cart);
                    await context.SaveChangesAsync();

                    var tokenInfo = user.ToTokenRequest();
                    string token = tokenService.CreateToken(tokenInfo!);
                    string rfToken = tokenService.CreateRefreshToken(tokenInfo!);
                    return Ok(new TokenResponse
                    {
                        Token = token,
                        RefreshToken = rfToken
                    });
                }

                //TH đăng nhập bằng phương thức GG
                user = await context.Users
                    .Include(u => u.Devices)
                    .Where(u => u.Email == ggResponse!.Email && u.LoginMethod == LoginMethod.Google)
                    .FirstOrDefaultAsync();
                if (user != null)
                {
                    //if (user.Status == UserStatus.Inactive)
                    //{
                    //    throw TechGadgetException.NewBuilder()
                    //        .WithCode(TechGadgetErrorCode.WEB_03)
                    //        .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
                    //        .Build();
                    //}

                    if (!request.DeviceToken.IsNullOrEmpty() && !user.Devices.Any(d => d.Token == request.DeviceToken))
                    {
                        context.Devices.Add(new Device
                        {
                            UserId = user.Id,
                            Token = request.DeviceToken!,
                        });
                        await context.SaveChangesAsync();
                    }

                    var tokenInfo = user.ToTokenRequest();
                    string token = tokenService.CreateToken(tokenInfo!);
                    string rfToken = tokenService.CreateRefreshToken(tokenInfo!);
                    return Ok(new TokenResponse
                    {
                        Token = token,
                        RefreshToken = rfToken
                    });
                }

                //TH mail này LoginMethod.Default
                user = await context.Users
                    .Where(u => u.Email == ggResponse!.Email && u.LoginMethod == LoginMethod.Default)
                    .FirstOrDefaultAsync();
                if (user != null)
                {
                    throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("google", "Người dùng này đã đăng nhập bình thường")
                        .Build();
                }

                throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("google", "Lỗi lạ không xác định")
                        .Build();
            }
        }
        catch (HttpRequestException ex)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("google", ex.Message)
                .Build();
        }
        catch (JsonException ex)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("google", ex.Message)
                .Build();
        }
    }
}
