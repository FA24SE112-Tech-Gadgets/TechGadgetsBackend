using WebApi.Data.Entities;
using WebApi.Services.Auth.Models;

namespace WebApi.Features.Auth.Mappers;

public static class UserMapper
{
    public static TokenRequest? ToTokenRequest(this User? user)
    {
        if (user != null)
        {
            return new TokenRequest
            {
                Email = user.Email,
                Id = user.Id,
                Role = user.Role,
                Status = user.Status,
            };
        }
        return null;
    }
}
