using WebApi.Data.Entities;
using WebApi.Features.Notificaitons.Models;

namespace WebApi.Features.Notifications.Mappers;

public static class CustomerNotiMapper
{
    public static CustomerNotiResponse? ToCustomerNotiResponse(this Customer c)
    {
        if (c != null)
        {
            return new CustomerNotiResponse
            {
                Id = c.Id,
                FullName = c.FullName,
            };
        }
        return null;
    }
}
