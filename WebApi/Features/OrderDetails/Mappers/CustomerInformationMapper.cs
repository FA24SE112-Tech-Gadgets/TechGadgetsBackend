using WebApi.Data.Entities;
using WebApi.Features.OrderDetails.Models;

namespace WebApi.Features.OrderDetails.Mappers;

public static class CustomerInformationMapper
{
    public static CustomerInfoResponse? ToCustomerInfoResponse(this CustomerInformation ci)
    {
        if (ci != null)
        {
            return new CustomerInfoResponse
            {
                FullName = ci.FullName,
                Address = ci.Address,
                PhoneNumber = ci.PhoneNumber,
            };
        }
        return null;
    }
}
