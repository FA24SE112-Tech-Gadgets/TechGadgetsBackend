using WebApi.Data.Entities;
using WebApi.Features.Reviews.Models;

namespace WebApi.Features.Reviews.Mappers;

public static class CustomerMapper
{
    public static CustomerReviewResponse? ToCustomerReviewResponse(this Customer customer)
    {
        if (customer != null)
        {
            return new CustomerReviewResponse
            {
                Id = customer.Id,
                UserId = customer.UserId,
                FullName = customer.FullName,
                AvatarUrl = customer.AvatarUrl,
            };
        }
        return null;
    }
}
