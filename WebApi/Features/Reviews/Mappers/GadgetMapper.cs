using WebApi.Data.Entities;
using WebApi.Features.Reviews.Models;

namespace WebApi.Features.Reviews.Mappers;

public static class GadgetMapper
{
    public static GadgetReviewResponse? ToGadgetReviewResponse(this Review review)
    {
        if (review != null)
        {
            //return new GadgetReviewResponse
            //{
            //    Id = review.GadgetId,
            //    Name = review.Gadget.Name,
            //    ThumbnailUrl = review.Gadget.ThumbnailUrl,
            //    Review = review.ToReviewResponse()!,
            //    Status = review.Gadget.Status,
            //};
            return null;
        }
        return null;
    }
}
