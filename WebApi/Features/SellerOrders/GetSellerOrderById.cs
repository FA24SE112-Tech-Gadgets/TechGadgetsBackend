using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.SellerOrders.Mappers;
using WebApi.Features.SellerOrders.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.SellerOrders;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class GetSellerOrderById : ControllerBase
{
    [HttpGet("seller-orders/{sellerOrderId}")]
    [Tags("Seller Orders")]
    [SwaggerOperation(
        Summary = "Get Seller Order Information By SellerOrderId",
        Description = "API is for get seller order information by sellerOrderId." +
                            "<br>&nbsp; - Customer dùng API này để xem chi tiết sellerOrder của mình." +
                            "<br>&nbsp; - Seller dùng API này để xem chi tiết sellerOrder liên quan đến mình."
    )]
    [ProducesResponseType(typeof(SellerOrderDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid sellerOrderId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var sellerOrder = await context.SellerOrders
            .Include(so => so.Order)
                .ThenInclude(o => o.WalletTracking)
            .Include(so => so.SellerOrderItems)
            .Include(so => so.CustomerInformation)
            .Include(so => so.SellerInformation)
            .FirstOrDefaultAsync(so => so.Id == sellerOrderId);

        var customerInfo = sellerOrder!.CustomerInformation;
        var sellerInfo = sellerOrder!.SellerInformation;

        if (sellerOrder == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEA_00)
            .AddReason("sellerOrder", "Không tìm thấy đơn hàng này.")
            .Build();
        }

        if ((currentUser!.Role == Role.Customer && sellerOrder!.Order.CustomerId != currentUser!.Customer!.Id) || (currentUser!.Role == Role.Seller && sellerOrder!.SellerId != currentUser!.Seller!.Id)) {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("orderDetail", "Người dùng không đủ thẩm quyền để truy cập đơn này.")
            .Build();
        }
        int totalQuantity = 0;
        int totalAmount = 0;
        foreach (var soi in sellerOrder.SellerOrderItems)
        {
            totalQuantity += soi.GadgetQuantity;
            totalAmount += (soi.GadgetQuantity * soi.GadgetPrice);
        }

        var walletTrackingCancel = await context.WalletTrackings.FirstOrDefaultAsync(wt => wt.Type == WalletTrackingType.Refund && wt.SellerOrderId == sellerOrderId);

        SellerOrderDetailResponse sellerOrderResponse = new SellerOrderDetailResponse()
        {
            Status = sellerOrder.Status,
            CustomerInfo = customerInfo!.ToCustomerInfoResponse()!,
            SellerInfo = sellerInfo!.ToSellerInfoResponse()!,
            TotalQuantity = totalQuantity,
            TotalAmount = totalAmount,
            OrderDetailId = sellerOrderId,
            OrderDetailCreatedAt = sellerOrder.CreatedAt,
            WalletTrackingCreatedAt = sellerOrder.Order.WalletTracking.CreatedAt,
            OrderDetailUpdatedAt = (sellerOrder.Status == SellerOrderStatus.Cancelled || sellerOrder.Status == SellerOrderStatus.Success) ? sellerOrder.UpdatedAt : null,
            CancelledReason = walletTrackingCancel != null ? walletTrackingCancel.Reason : null,
        }!;

        return Ok(sellerOrderResponse);
    }
}
