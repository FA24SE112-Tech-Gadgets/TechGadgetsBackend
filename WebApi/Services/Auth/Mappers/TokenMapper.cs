using WebApi.Data.Entities;
using WebApi.Services.Auth.Models;

namespace WebApi.Services.Auth.Mappers;

public static class TokenMapper
{
    public static User? ToUser(this TokenRequest? tokenRequest)
    {
        if (tokenRequest == null)
        {
            return null;
        }
        return new User
        {
            Email = tokenRequest.Email,
            Id = tokenRequest.Id,
            Role = tokenRequest.Role,
            LoginMethod = tokenRequest.LoginMethod,
            Status = tokenRequest.Status,
        };
    }
}
