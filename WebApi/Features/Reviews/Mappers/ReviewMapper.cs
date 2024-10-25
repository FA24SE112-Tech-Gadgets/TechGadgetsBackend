using WebApi.Data.Entities;
using WebApi.Features.Reviews.Models;

namespace WebApi.Features.Reviews.Mappers;

public static class ReviewMapper
{
    public static ReviewResponse? ToReviewResponse(this Review review)
    {
        if (review != null)
        {
            return new ReviewResponse
            {
                Id = review.Id,
                Customer = review.Customer.ToCustomerReviewResponse()!,
                Rating = review.Rating,
                Content = review.Content,
                IsPositive = review.IsPositive,
                Status = review.Status,
                CreatedAt = review.CreatedAt,
                IsUpdated = review.UpdatedAt != review.CreatedAt
            };
        }
        return null;
    }
}
