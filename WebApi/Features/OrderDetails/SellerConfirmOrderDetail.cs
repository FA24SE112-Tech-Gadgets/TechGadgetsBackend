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
[RolesFilter(Role.Seller)]
public class SellerConfirmOrderDetail : ControllerBase
{
    [HttpPut("order-details/{orderDetailId}/confirm")]
    [Tags("Order Details")]
    [SwaggerOperation(
        Summary = "Confirm Order Detail By OrderDetailId",
        Description = "API is for cancel order detail by orderDetailId." +
                            "<br>&nbsp; - Seller chỉ được confirm orderDetail mà liên quan đến bản thân" +
                            "<br>&nbsp; - Không thể confirm những orderDetail đã Canceled." +
                            "<br>&nbsp; - Sau khi orderDetail confirm success thì hệ thống sẽ tự động sẽ tự động chuyển tiền vào ví Seller."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid orderDetailId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
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

        if (orderDetail!.SellerId != currentUser!.Seller!.Id)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("orderDetail", "Người dùng không đủ thẩm quyền để truy cập đơn này.")
            .Build();
        }

        orderDetail!.Status = OrderDetailStatus.Success;

        WalletTracking walletTracking = new WalletTracking()
        {
            OrderDetailId = orderDetailId,
            Amount = orderDetail.Amount,
            Type = WalletTrackingType.SellerTransfer,
            Status = WalletTrackingStatus.Pending,
            CreatedAt = DateTime.UtcNow,
        }!;

        await context.WalletTrackings.AddAsync(walletTracking);
        await context.SaveChangesAsync();

        return Ok();
    }
}
