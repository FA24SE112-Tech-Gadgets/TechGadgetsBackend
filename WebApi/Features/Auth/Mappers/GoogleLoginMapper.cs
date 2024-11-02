using WebApi.Data.Entities;
using WebApi.Features.Auth.Models;

namespace WebApi.Features.Auth.Mappers;

public static class GoogleLoginMapper
{
    public static User? ToUserRequest(this RegisterUserRequest? registerUserRequest)
    {
        if (registerUserRequest == null)
        {
            return null;
        }
        return new User
        {
            Email = registerUserRequest.Email,
            Role = registerUserRequest.Role,
            LoginMethod = registerUserRequest.LoginMethod,
            Status = registerUserRequest.Status,
            Customer = new Customer
            {
                FullName = registerUserRequest.FullName,
                AvatarUrl = registerUserRequest.AvatarUrl,
            },
            Devices = registerUserRequest.Devices,
        };
    }
}
