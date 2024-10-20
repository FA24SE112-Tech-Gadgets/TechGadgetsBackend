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
                Id = gadgetInformation.Id,
                OrderDetailId = gadgetInformation.OrderDetailId,
                GadgetThumbnailUrl = gadgetInformation.GadgetThumbnailUrl,
                GadgetName = gadgetInformation.GadgetName,
                GadgetPrice = gadgetInformation.GadgetPrice,
                GadgetQuantity = gadgetInformation.GadgetQuantity,
                GadgetId = gadgetInformation.GadgetId,
            };
        }
        return null;
    }
    private static SellerOrderDetailResponse? ToSellerOrderDetailResponse(this Seller seller)
    {
        if (seller != null)
        {
            return new SellerOrderDetailResponse
            {
                Id = seller.Id,
                CompanyName = seller.CompanyName,
                ShopName = seller.ShopName,
                ShopAddress = seller.ShopAddress,
                BusinessModel = seller.BusinessModel,
                PhoneNumber = seller.PhoneNumber,
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

    public static CustomerOrderDetailItemResponse? ToCustomerOrderDetailItemResponse(this OrderDetail orderDetail)
    {
        if (orderDetail != null)
        {
            return new CustomerOrderDetailItemResponse
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                Amount = orderDetail.Amount,
                Seller = orderDetail.Seller.ToSellerOrderDetailResponse()!,
                Status = orderDetail.Status,
                GadgetInformation = orderDetail.GadgetInformation.ToListGadgetInformations()!,
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
