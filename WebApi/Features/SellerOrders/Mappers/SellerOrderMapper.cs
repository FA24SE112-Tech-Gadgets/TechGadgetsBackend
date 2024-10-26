using WebApi.Data.Entities;
using WebApi.Features.SellerOrders.Models;

namespace WebApi.Features.SellerOrders.Mappers;

public static class SellerOrderMapper
{
    private static SellerOrderItemInItemResponse? ToGadgetInformationOrderDetailResponse(this SellerOrderItem gadgetInformation)
    {
        if (gadgetInformation != null)
        {
            return new SellerOrderItemInItemResponse
            {
                Id = gadgetInformation.GadgetId,
                Price = gadgetInformation.GadgetPrice,
                Quantity = gadgetInformation.GadgetQuantity,
            };
        }
        return null;
    }

    private static List<SellerOrderItemInItemResponse>? ToListGadgetInformations(this ICollection<SellerOrderItem> gadgetInformations)
    {
        // Kiểm tra nếu gadgetInformations không null và có ít nhất một phần tử
        if (gadgetInformations.Count() > 0)
        {
            // Lấy phần tử đầu tiên và chuyển đổi nó thành GadgetInformationOrderDetailResponse, rồi đưa vào một danh sách mới
            return new List<SellerOrderItemInItemResponse>
            {
                gadgetInformations.First().ToGadgetInformationOrderDetailResponse()!
            }!;
        }

        // Trả về null nếu không có phần tử nào
        return null;
    }

    public static List<SellerOrderItemInItemResponse>? ToListSellerOrderItemsDetail(this ICollection<SellerOrderItem> gadgetInformations)
    {
        if (gadgetInformations != null && gadgetInformations.Count > 0)
        {
            return gadgetInformations
            .Select(gi => new SellerOrderItemInItemResponse
            {
                Id = gi.GadgetId,
                Price = gi.GadgetPrice,
                Quantity = gi.GadgetQuantity,
            })
            .ToList();
        }
        return null;
    }

    public static CustomerSellerOrderItemResponse? ToCustomerOrderDetailItemResponse(this SellerOrder orderDetail)
    {
        if (orderDetail != null)
        {
            return new CustomerSellerOrderItemResponse
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                Status = orderDetail.Status,
                Gadgets = orderDetail.SellerOrderItems.ToListGadgetInformations()!,
                CreatedAt = orderDetail.CreatedAt,
            };
        }
        return null;
    }

    public static SellerOrderResponse? ToSellerOrderDetailItemResponse(this SellerOrder orderDetail)
    {
        if (orderDetail != null)
        {
            return new SellerOrderResponse
            {
                Id = orderDetail.Id,
                Status = orderDetail.Status,
                CreatedAt = orderDetail.CreatedAt,
            };
        }
        return null;
    }
}
