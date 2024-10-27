using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.SellerOrders.Mappers;
using WebApi.Features.SellerOrders.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.SellerOrders;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class GetListSellerOrderItemsBySellerOrderId : ControllerBase
{
    [HttpGet("seller-order/{sellerOrderId}/items")]
    [Tags("Seller Orders")]
    [SwaggerOperation(
        Summary = "Get List Items In Seller Order By SellerOrderId",
        Description = "API is for get list items in sellerOrder by sellerOrderId." +
                            "<br>&nbsp; - Customer dùng API này để xem danh sách gadgets có trong sellerOrder của mình." +
                            "<br>&nbsp; - Seller dùng API này để xem danh sách gadgets có trong sellerOrder liên quan đến mình." +
                            "<br>&nbsp; - SellerOrderItemId khác với GadgetId."
    )]
    [ProducesResponseType(typeof(PagedList<SellerOrderItemInItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid sellerOrderId, [FromQuery] PagedRequest pagedRequest, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var sellerOrder = await context.SellerOrders
            .Include(so => so.Order)
            .Include(so => so.SellerOrderItems)
                .ThenInclude(soi => soi.Gadget)
            .FirstOrDefaultAsync(so => so.Id == sellerOrderId);

        if (sellerOrder == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEA_00)
            .AddReason("sellerOrder", "Không tìm thấy đơn hàng này.")
            .Build();
        }

        if ((currentUser!.Role == Role.Customer && sellerOrder!.Order.CustomerId != currentUser!.Customer!.Id) || (currentUser!.Role == Role.Seller && sellerOrder!.SellerId != currentUser!.Seller!.Id))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("sellerOrder", "Người dùng không đủ thẩm quyền để truy cập đơn này.")
            .Build();
        }

        var gadgetInformations = await context.SellerOrderItems
            .Where(gi => gi.SellerOrderId == sellerOrderId)
            .ToPagedListAsync(pagedRequest);

        var gadgetInformationsResponseList = new PagedList<SellerOrderItemInItemResponse>(
            gadgetInformations.Items.ToListSellerOrderItemsDetail()!,
            gadgetInformations.Page,
            gadgetInformations.PageSize,
            gadgetInformations.TotalItems
        );

        return Ok(gadgetInformationsResponseList);
    }
}
