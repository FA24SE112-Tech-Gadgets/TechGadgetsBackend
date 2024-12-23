﻿using WebApi.Data.Entities;
using WebApi.Features.SellerOrders.Models;

namespace WebApi.Features.SellerOrders.Mappers;

public static class SellerOrderMapper
{
    private static SellerOrderItemInItemResponse? ToGadgetInformationOrderDetailResponse(this SellerOrderItem sellerOrderItem)
    {
        if (sellerOrderItem != null)
        {
            int discountPercentage = sellerOrderItem.GadgetDiscount != null ? sellerOrderItem.GadgetDiscount.DiscountPercentage : 0;
            return new SellerOrderItemInItemResponse
            {
                SellerOrderItemId = sellerOrderItem.Id,
                GadgetId = sellerOrderItem.GadgetId,
                Name = sellerOrderItem.Gadget.Name,
                Price = sellerOrderItem.GadgetPrice,
                DiscountPrice = (int)Math.Ceiling(sellerOrderItem.GadgetPrice * (1 - discountPercentage / 100.0)),
                DiscountPercentage = discountPercentage,
                ThumbnailUrl = sellerOrderItem.Gadget.ThumbnailUrl,
                Quantity = sellerOrderItem.GadgetQuantity,
                GadgetStatus = sellerOrderItem.Gadget.Status,
                SellerStatus = sellerOrderItem.Gadget.Seller.User.Status
            };
        }
        return null;
    }

    private static List<SellerOrderItemInItemResponse>? ToListSellerOrderItems(this ICollection<SellerOrderItem> sellerOrderItem)
    {
        // Kiểm tra nếu gadgetInformations không null và có ít nhất một phần tử
        if (sellerOrderItem.Count() > 0)
        {
            // Lấy phần tử đầu tiên và chuyển đổi nó thành GadgetInformationOrderDetailResponse, rồi đưa vào một danh sách mới
            return new List<SellerOrderItemInItemResponse>
            {
                sellerOrderItem.First().ToGadgetInformationOrderDetailResponse()!
            }!;
        }

        // Trả về null nếu không có phần tử nào
        return null;
    }

    public static List<SellerOrderItemInItemResponse>? ToListSellerOrderItemsDetail(this ICollection<SellerOrderItem> sellerOrderItems)
    {
        if (sellerOrderItems != null && sellerOrderItems.Count > 0)
        {
            return sellerOrderItems
            .Select(soi =>
            {
                int discountPercentage = soi.GadgetDiscount != null ? soi.GadgetDiscount.DiscountPercentage : 0;
                return new SellerOrderItemInItemResponse
                {
                    SellerOrderItemId = soi.Id,
                    GadgetId = soi.GadgetId,
                    Name = soi.Gadget.Name,
                    Price = soi.GadgetPrice,
                    DiscountPrice = (int)Math.Ceiling(soi.GadgetPrice * (1 - discountPercentage / 100.0)),
                    DiscountPercentage = discountPercentage,
                    ThumbnailUrl = soi.Gadget.ThumbnailUrl,
                    Quantity = soi.GadgetQuantity,
                    GadgetStatus = soi.Gadget.Status,
                    SellerStatus = soi.Gadget.Seller.User.Status
                };
            })
            .ToList();
        }
        return null;
    }

    public static CustomerSellerOrderItemResponse? ToCustomerSellerOrderItemResponse(this SellerOrder sellerOrder)
    {
        if (sellerOrder != null)
        {
            return new CustomerSellerOrderItemResponse
            {
                Id = sellerOrder.Id,
                OrderId = sellerOrder.OrderId,
                Status = sellerOrder.Status,
                Gadgets = sellerOrder.SellerOrderItems.ToListSellerOrderItems()!,
                CreatedAt = sellerOrder.CreatedAt,
            };
        }
        return null;
    }

    public static SellerOrderResponse? ToSellerSellerOrderItemResponse(this SellerOrder sellerOrder)
    {
        if (sellerOrder != null)
        {
            long totalAmount = 0;
            long totalDiscount = 0;
            int totalQuantity = 0;
            foreach (var soi in sellerOrder.SellerOrderItems)
            {
                int discountPercentage = soi.GadgetDiscount != null ? soi.GadgetDiscount.DiscountPercentage : 0;
                totalDiscount += (long)Math.Ceiling(soi.GadgetPrice * discountPercentage / 100.0) * soi.GadgetQuantity;
                totalAmount += (long)Math.Ceiling(soi.GadgetPrice * (1 - discountPercentage / 100.0)) * soi.GadgetQuantity;
                totalQuantity += soi.GadgetQuantity;
            }
            return new SellerOrderResponse
            {
                Id = sellerOrder.Id,
                Customer = sellerOrder.CustomerInformation.ToCustomerInfoResponse()!,
                Amount = totalAmount,
                DiscountAmount = totalDiscount,
                BeforeAppliedDiscountAmount = totalAmount + totalDiscount,
                TotalQuantity = totalQuantity,
                Status = sellerOrder.Status,
                CreatedAt = sellerOrder.CreatedAt,
            };
        }
        return null;
    }
}
