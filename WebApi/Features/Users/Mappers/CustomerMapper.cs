using WebApi.Data.Entities;
using WebApi.Features.Users.Models;

namespace WebApi.Features.Users.Mappers;

public static class CustomerMapper
{
    public static CustomerResponse? ToCustomerResponse(this Customer? customer)
    {
        if (customer != null)
        {
            return new CustomerResponse
            {
                Id = customer.Id,
                FullName = customer.FullName,
                UserId = customer.UserId,
                Gender = customer.Gender,
                Address = customer.Address,
                AvatarUrl = customer.AvatarUrl,
                CCCD = customer.CCCD,
                DateOfBirth = customer.DateOfBirth,
                PhoneNumber = customer.PhoneNumber
            };
        }
        return null;
    }
}
