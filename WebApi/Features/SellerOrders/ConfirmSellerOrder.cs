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
[RolesFilter(Role.Seller)]
public class ConfirmSellerOrder : ControllerBase
{
    [HttpPut("seller-order/{sellerOrderId}/confirm")]
    [Tags("Seller Orders")]
    [SwaggerOperation(
        Summary = "Confirm Seller Order By SellerOrderId",
        Description = "API is for cancel selelr order by sellerOrderId." +
                            "<br>&nbsp; - Seller chỉ được confirm selelrOrder mà liên quan đến bản thân" +
                            "<br>&nbsp; - Không thể confirm những selelrOrder đã Canceled." +
                            "<br>&nbsp; - Sau khi selelrOrder confirm success thì hệ thống sẽ tự động sẽ tự động chuyển tiền vào ví Seller."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid sellerOrderId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var sellerOrder = await context.SellerOrders
            .Include(so => so.Order)
            .FirstOrDefaultAsync(so => so.Id == sellerOrderId);
        if (sellerOrder == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerOrder", "Không tìm thấy đơn này.")
            .Build();
        }

        if (sellerOrder!.SellerId != currentUser!.Seller!.Id)
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

        sellerOrder!.Status = SellerOrderStatus.Success;

        var userWallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == currentUser!.Id);

        int totalAmount = 0;
        WalletTracking walletTracking = new WalletTracking()
        {
            WalletId = userWallet!.Id,
            SellerOrderId = sellerOrderId,
            Type = WalletTrackingType.SellerTransfer,
            Status = WalletTrackingStatus.Pending,
            CreatedAt = DateTime.UtcNow,
        }!;

        //Tính tổng cho Wallettracking
        var selelrOrderItems = sellerOrder.SellerOrderItems;
        foreach (var soi in selelrOrderItems)
        {
            totalAmount += (soi.GadgetPrice * soi.GadgetQuantity);
        }

        walletTracking.Amount = totalAmount;

        await context.WalletTrackings.AddAsync(walletTracking);
        await context.SaveChangesAsync();

        return Ok();
    }
}
