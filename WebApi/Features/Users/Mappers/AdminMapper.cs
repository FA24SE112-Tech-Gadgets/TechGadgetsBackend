using WebApi.Data.Entities;
using WebApi.Features.Users.Models;

namespace WebApi.Features.Users.Mappers;

public static class AdminMapper
{
    public static AdminResponse? ToAdminResponse(this Admin? admin)
    {
        if (admin != null)
        {
            return new AdminResponse
            {
                Id = admin.Id,
                FullName = admin.FullName,
                UserId = admin.UserId,
            };
        }
        return null;
    }
}
