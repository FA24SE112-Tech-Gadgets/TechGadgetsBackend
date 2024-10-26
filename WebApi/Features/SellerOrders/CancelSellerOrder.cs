using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.SellerOrders;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class CancelSellerOrder : ControllerBase
{
    public new class Request
    {
        public string? Reason { get; set; } = default!;
    }

    [HttpPut("seller-order/{sellerOrderId}/cancel")]
    [Tags("Seller Orders")]
    [SwaggerOperation(
        Summary = "Cancel Selelr Order By SellerOrderId",
        Description = "API is for cancel seller order by sellerOrderId." +
                            "<br>&nbsp; - Customer chỉ được cancel sellerOrder trước khi Seller confirm" +
                            "<br>&nbsp; - Seller và Customer không thể cancel sellerOrder khi đơn đã hoàn thành." +
                            "<br>&nbsp; - Sau khi sellerOrder canceled success thì hệ thống sẽ tự động hoàn tiền sau 1 phút."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromRoute] Guid sellerOrderId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var sellerOrder = await context.SellerOrders
            .Include(so => so.Order)
            .Include(so => so.SellerOrderItems)
                .ThenInclude(gi => gi.Gadget)
            .FirstOrDefaultAsync(so => so.Id == sellerOrderId);
        if (sellerOrder == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerOrder", "Không tìm thấy đơn này.")
            .Build();
        }

        if ((currentUser!.Role == Role.Customer && sellerOrder!.Order.CustomerId != currentUser!.Customer!.Id) || (currentUser!.Role == Role.Seller && sellerOrder!.SellerId != currentUser!.Seller!.Id))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("sellerOrder", "Người dùng không đủ thẩm quyền để truy cập đơn này.")
            .Build();
        }

        if (sellerOrder.Status != SellerOrderStatus.Pending)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerOrder", "Đơn này đã Success/Cancelled rồi.")
            .Build();
        }

        sellerOrder!.Status = SellerOrderStatus.Cancelled;

        var userWallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == currentUser!.Id);

        int totalAmount = 0;
        WalletTracking walletTracking = new WalletTracking()
        {
            WalletId = userWallet!.Id,
            SellerOrderId = sellerOrderId,
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
                .AddReason("sellerOrder", "Lý do không được để trống.")
                .Build();
            }
            walletTracking.Reason = request.Reason;
        }


        //Hoàn lại quantity cho gadget của Seller và tính tổng cho Wallettracking
        var selelrOrderItems = sellerOrder.SellerOrderItems;
        foreach (var soi in selelrOrderItems)
        {
            totalAmount += (soi.GadgetPrice * soi.GadgetQuantity);
            soi.Gadget.Quantity += soi.GadgetQuantity;
        }

        walletTracking.Amount = totalAmount;
        await context.WalletTrackings.AddAsync(walletTracking);
        await context.SaveChangesAsync();

        return Ok();
    }
}
