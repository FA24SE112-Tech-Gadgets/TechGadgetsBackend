using WebApi.Data.Entities;
using WebApi.Features.Users.Models;

namespace WebApi.Features.Users.Mappers;

public static class ManagerMapper
{
    public static ManagerResponse? ToManagerResponse(this Manager? manager)
    {
        if (manager != null)
        {
            return new ManagerResponse
            {
                Id = manager.Id,
                FullName = manager.FullName,
                UserId = manager.UserId,
            };
        }
        return null;
    }
}
