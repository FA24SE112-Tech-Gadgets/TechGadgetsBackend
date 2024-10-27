using WebApi.Data.Entities;
using WebApi.Features.Reviews.Models;

namespace WebApi.Features.Reviews.Mappers;

public static class SellerReplyMapper
{
    public static SellerReplyResponse? ToSellerReplyResponse(this SellerReply sellerReply)
    {
        if (sellerReply != null)
        {
            return new SellerReplyResponse
            {
                Id = sellerReply.Id,
                Seller = sellerReply.Seller.ToSellerResponse()!,
                Content = sellerReply.Content,
                Status = sellerReply.Status,
                CreatedAt = sellerReply.CreatedAt,
                IsUpdated = sellerReply.UpdatedAt != sellerReply.CreatedAt
            };
        }
        return null;
    }
}
