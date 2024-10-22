using WebApi.Data.Entities;
using WebApi.Features.OrderDetails.Models;

namespace WebApi.Features.OrderDetails.Mappers;

public static class OrderDetailMapper
{
    private static GadgetInformationOrderDetailResponse? ToGadgetInformationOrderDetailResponse(this GadgetInformation gadgetInformation)
    {
        if (gadgetInformation != null)
        {
            return new GadgetInformationOrderDetailResponse
            {
                Id = gadgetInformation.GadgetId,
                Name = gadgetInformation.GadgetName,
                Price = gadgetInformation.GadgetPrice,
                ThumbnailUrl = gadgetInformation.GadgetThumbnailUrl,
                Quantity = gadgetInformation.GadgetQuantity,
            };
        }
        return null;
    }

    private static List<GadgetInformationOrderDetailResponse>? ToListGadgetInformations(this ICollection<GadgetInformation> gadgetInformations)
    {
        // Kiểm tra nếu gadgetInformations không null và có ít nhất một phần tử
        if (gadgetInformations.Count() > 0)
        {
            // Lấy phần tử đầu tiên và chuyển đổi nó thành GadgetInformationOrderDetailResponse, rồi đưa vào một danh sách mới
            return new List<GadgetInformationOrderDetailResponse>
            {
                gadgetInformations.First().ToGadgetInformationOrderDetailResponse()!
            }!;
        }

        // Trả về null nếu không có phần tử nào
        return null;
    }

    public static List<GadgetInformationOrderDetailResponse>? ToListGadgetInformationsDetail(this ICollection<GadgetInformation> gadgetInformations)
    {
        if (gadgetInformations != null && gadgetInformations.Count > 0)
        {
            return gadgetInformations
            .Select(gi => new GadgetInformationOrderDetailResponse
            {
                Id = gi.GadgetId,
                Name = gi.GadgetName,
                Price = gi.GadgetPrice,
                Quantity = gi.GadgetQuantity,
                ThumbnailUrl = gi.GadgetThumbnailUrl,
            })
            .ToList();
        }
        return null;
    }

    public static CustomerOrderDetailItemResponse? ToCustomerOrderDetailItemResponse(this OrderDetail orderDetail)
    {
        if (orderDetail != null)
        {
            return new CustomerOrderDetailItemResponse
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                Amount = orderDetail.Amount,
                Status = orderDetail.Status,
                Gadgets = orderDetail.GadgetInformation.ToListGadgetInformations()!,
                CreatedAt = orderDetail.CreatedAt,
            };
        }
        return null;
    }

    public static SellerOrderDetailItemResponse? ToSellerOrderDetailItemResponse(this OrderDetail orderDetail)
    {
        if (orderDetail != null)
        {
            return new SellerOrderDetailItemResponse
            {
                Id = orderDetail.Id,
                Amount = orderDetail.Amount,
                Status = orderDetail.Status,
                CreatedAt = orderDetail.CreatedAt,
            };
        }
        return null;
    }
}
