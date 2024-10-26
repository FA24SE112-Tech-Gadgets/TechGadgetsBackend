using WebApi.Data.Entities;
using WebApi.Features.SellerOrders.Models;

namespace WebApi.Features.SellerOrders.Mappers;

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
