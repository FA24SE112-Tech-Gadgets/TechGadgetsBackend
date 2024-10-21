using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.OrderDetails;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class CancelOrderDetail : ControllerBase
{
    public new class Request
    {
        public string? Reason { get; set; } = default!;
    }

    [HttpPut("order-details/{orderDetailId}/cancel")]
    [Tags("Order Details")]
    [SwaggerOperation(
        Summary = "Cancel Order Detail By OrderDetailId",
        Description = "API is for cancel order detail by orderDetailId." +
                            "<br>&nbsp; - Customer chỉ được cancel orderDetail trước khi Seller confirm" +
                            "<br>&nbsp; - Seller và Customer không thể cancel orderDetail khi đơn đã hoàn thành." +
                            "<br>&nbsp; - Sau khi orderDetail canceled success thì hệ thống sẽ tự động hoàn tiền sau 1 phút."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromRoute] Guid orderDetailId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var orderDetail = await context.OrderDetails
            .Include(od => od.Order)
            .FirstOrDefaultAsync(od => od.Id == orderDetailId);
        if (orderDetail == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("orderDetail", "Không tìm thấy đơn này.")
            .Build();
        }

        if ((currentUser!.Role == Role.Customer && orderDetail!.Order.CustomerId != currentUser!.Customer!.Id) || (currentUser!.Role == Role.Seller && orderDetail!.SellerId != currentUser!.Seller!.Id))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("orderDetail", "Người dùng không đủ thẩm quyền để truy cập đơn này.")
            .Build();
        }

        orderDetail!.Status = OrderDetailStatus.Cancelled;

        WalletTracking walletTracking = new WalletTracking()
        {
            OrderDetailId = orderDetailId,
            Amount = orderDetail.Amount,
            Type = WalletTrackingType.Refund,
            Status = WalletTrackingStatus.Pending,
            CreatedAt = DateTime.UtcNow,
        }!;

        if (currentUser!.Role == Role.Customer)
        {
            walletTracking.Reason = "Khách hàng yêu cầu hủy đơn";
        } else
        {
            if (request.Reason == null)
            {
                throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("orderDetail", "Lý do không được để trống.")
                .Build();
            }
            walletTracking.Reason = request.Reason;
        }

        await context.WalletTrackings.AddAsync(walletTracking);
        await context.SaveChangesAsync();

        return Ok();
    }
}
