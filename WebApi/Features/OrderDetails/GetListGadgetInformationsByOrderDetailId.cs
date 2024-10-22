using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.OrderDetails.Mappers;
using WebApi.Features.OrderDetails.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.OrderDetails;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class GetListGadgetInformationsByOrderDetailId : ControllerBase
{
    [HttpGet("order-details/{orderDetailId}/gadgets")]
    [Tags("Order Details")]
    [SwaggerOperation(
        Summary = "Get List Gadgets In Order Detail By OrderDetailId",
        Description = "API is for get list gadgets in orderDetail by orderDetailId." +
                            "<br>&nbsp; - Customer dùng API này để xem danh sách gadgets có trong orderDetail của mình." +
                            "<br>&nbsp; - Seller dùng API này để xem danh sách gadgets có trong orderDetail liên quan đến mình."
    )]
    [ProducesResponseType(typeof(PagedList<GadgetInformationOrderDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid orderDetailId, [FromQuery] PagedRequest pagedRequest, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var orderDetail = await context.OrderDetails
            .Include(od => od.Order)
            .FirstOrDefaultAsync(od => od.Id == orderDetailId);

        if (orderDetail == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEA_00)
            .AddReason("orderDetail", "Không tìm thấy đơn hàng này.")
            .Build();
        }

        if ((currentUser!.Role == Role.Customer && orderDetail!.Order.CustomerId != currentUser!.Customer!.Id) || (currentUser!.Role == Role.Seller && orderDetail!.SellerId != currentUser!.Seller!.Id))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("orderDetail", "Người dùng không đủ thẩm quyền để truy cập đơn này.")
            .Build();
        }

        var gadgetInformations = await context.GadgetInformation
            .Where(gi => gi.OrderDetailId == orderDetailId)
            .ToPagedListAsync(pagedRequest);

        var gadgetInformationsResponseList = new PagedList<GadgetInformationOrderDetailResponse>(
            gadgetInformations.Items.ToListGadgetInformationsDetail()!,
            gadgetInformations.Page,
            gadgetInformations.PageSize,
            gadgetInformations.TotalItems
        );

        return Ok(gadgetInformationsResponseList);
    }
}
