using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.OrderDetails.Mappers;
using WebApi.Services.Auth;

namespace WebApi.Features.OrderDetails;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class GetOrderDetailInDetail : ControllerBase
{
    [HttpGet("order-details/{orderDetailId}")]
    [Tags("Order Details")]
    [SwaggerOperation(
        Summary = "Get Order Details In Detail By OrderDetailId",
        Description = "API is for get order detail by orderDetailId." +
                            "<br>&nbsp; - Customer dùng API này để xem chi tiết orderDetail của mình." +
                            "<br>&nbsp; - Seller dùng API này để xem chi tiết orderDetail liên quan đến mình." +
                            "<br>&nbsp; - Response của Seller và Customer là khác nhau, nên gọi thử để biết thêm chi tiết."
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
            .Include(od => od.Seller)
            .Include(od => od.GadgetInformation)
            .FirstOrDefaultAsync(od => od.Id == orderDetailId);

        if (orderDetail == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEA_00)
            .AddReason("orderDetail", "Không tìm thấy đơn hàng này.")
            .Build();
        }

        if ((currentUser!.Role == Role.Customer && orderDetail!.Order.CustomerId != currentUser!.Customer!.Id) || (currentUser!.Role == Role.Seller && orderDetail!.SellerId != currentUser!.Seller!.Id)) {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("orderDetail", "Người dùng không đủ thẩm quyền để truy cập đơn này.")
            .Build();
        }

        if (currentUser!.Role == Role.Seller)
        {
            return Ok(orderDetail.ToSellerOrderDetailItemResponse());
        }
        else
        {
            return Ok(orderDetail.ToCustomerOrderDetailItemResponse());
        }
    }
}
