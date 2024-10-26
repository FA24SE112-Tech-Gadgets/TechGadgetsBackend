using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.OrderDetails.Mappers;
using WebApi.Features.OrderDetails.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.OrderDetails;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class GetOrderDetailById : ControllerBase
{
    [HttpGet("seller-orders/{sellerOrderId}")]
    [Tags("Seller Orders")]
    [SwaggerOperation(
        Summary = "Get Order Detail Information By SellerOrderId",
        Description = "API is for get order detail information by sellerOrderId." +
                            "<br>&nbsp; - Customer dùng API này để xem chi tiết sellerOrder của mình." +
                            "<br>&nbsp; - Seller dùng API này để xem chi tiết sellerOrder liên quan đến mình."
    )]
    [ProducesResponseType(typeof(OrderDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid sellerOrderId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var orderDetail = await context.SellerOrders
            .Include(od => od.Order)
                .ThenInclude(o => o.WalletTracking)
            .Include(od => od.SellerOrderItems)
            .Include(so => so.CustomerInformation)
            .Include(so => so.SellerInformation)
            .FirstOrDefaultAsync(od => od.Id == sellerOrderId);

        var customerInfo = orderDetail!.CustomerInformation;
        var sellerInfo = orderDetail!.SellerInformation;

        if (orderDetail == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEA_00)
            .AddReason("orderDetail", "Không tìm thấy đơn hàng này.")
            .Build();
        }

        if ((currentUser!.Role == Role.Customer && orderDetail!.Order.CustomerId != currentUser!.Customer!.Id) || (currentUser!.Role == Role.Seller && orderDetail!.SellerId != currentUser!.Seller!.Id)) {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("orderDetail", "Người dùng không đủ thẩm quyền để truy cập đơn này.")
            .Build();
        }
        int totalQuantity = 0;
        int totalAmount = 0;
        foreach (var gi in orderDetail.SellerOrderItems)
        {
            totalQuantity += gi.GadgetQuantity;
            totalAmount += (gi.GadgetQuantity * gi.GadgetPrice);
        }

        var walletTrackingCancel = await context.WalletTrackings.FirstOrDefaultAsync(wt => wt.Type == WalletTrackingType.Refund && wt.SellerOrderId == sellerOrderId);

        OrderDetailResponse orderDetailResponse = new OrderDetailResponse()
        {
            Status = orderDetail.Status,
            CustomerInfo = customerInfo!.ToCustomerInfoResponse()!,
            SellerInfo = sellerInfo!.ToSellerInfoResponse()!,
            TotalQuantity = totalQuantity,
            TotalAmount = totalAmount,
            OrderDetailId = sellerOrderId,
            OrderDetailCreatedAt = orderDetail.CreatedAt,
            WalletTrackingCreatedAt = orderDetail.Order.WalletTracking.CreatedAt,
            OrderDetailUpdatedAt = (orderDetail.Status == SellerOrderStatus.Cancelled || orderDetail.Status == SellerOrderStatus.Success) ? orderDetail.UpdatedAt : null,
            CancelledReason = walletTrackingCancel != null ? walletTrackingCancel.Reason : null,
        }!;

        return Ok(orderDetailResponse);
    }
}
