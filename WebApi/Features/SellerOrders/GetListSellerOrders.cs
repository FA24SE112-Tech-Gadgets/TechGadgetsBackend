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
public class GetListSellerOrders : ControllerBase
{
    public new class Request : PagedRequest
    {
        public SortByDate SortByDate { get; set; }
        public SellerOrderStatus? Status { get; set; }
    }

    public enum SortByDate
    {
        DESC, ASC
    }

    [HttpGet("seller-orders")]
    [Tags("Seller Orders")]
    [SwaggerOperation(
        Summary = "Get List Of Seller Orders",
        Description = "API is for get list of seller orders." +
                            "<br>&nbsp; - SortByDate: 'DESC' - Ngày gần nhất, 'ASC' - Ngày xa nhất. Nếu không truyền defaul: 'DESC'" +
                            "<br>&nbsp; - Status: 'Success', 'Pending', 'Cancelled'." +
                            "<br>&nbsp; - Customer dùng API này để lấy ra danh sách orderDetail của mình." +
                            "<br>&nbsp; - Seller dùng API này để lấy ra những orderDetail liên quan đến mình." +
                            "<br>&nbsp; - Response của Seller và Customer là khác nhau, nên gọi thử để biết thêm chi tiết." +
                            "<br>&nbsp; - LƯU Ý: Trong API này Gadgets chỉ lấy ra gadget đầu tiên, nên muốn xem danh sách gadget có trong order thì gọi API khác."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var query = context.SellerOrders.AsQueryable();

        if (currentUser!.Role == Role.Seller)
        {
            query = query.Where(od => od.SellerId == currentUser.Seller!.Id);
        } else
        {
            query = query
                .Include(od => od.Order)
                .Include(od => od.Seller)
                .Include(od => od.SellerOrderItems)
                .Include(so => so.SellerInformation)
                .Where(od => od.Order.CustomerId == currentUser.Customer!.Id);
        }

        if (request.Status != null)
        {
            query = query.Where(od => od.Status == request.Status);
        }

        if (request.SortByDate == SortByDate.DESC)
        {
            // Thêm sắp xếp theo CreatedAt (giảm dần, gần nhất trước)
            query = query.OrderByDescending(od => od.CreatedAt);
        }
        else
        {
            query = query.OrderBy(od => od.CreatedAt);
        }

        var orderDetails = await query
            .ToPagedListAsync(request);

        if (currentUser!.Role == Role.Seller)
        {
            var orderDetailsResponseList = new PagedList<SellerOrderResponse>(
                orderDetails.Items.Select(od => od.ToSellerOrderDetailItemResponse()!).ToList(),
                orderDetails.Page,
                orderDetails.PageSize,
                orderDetails.TotalItems
            );
            return Ok(orderDetailsResponseList);
        } else
        {
            List<CustomerSellerOrderItemResponse> customerOrderDetailItemResponses = new List<CustomerSellerOrderItemResponse>()!;
            foreach (var od in orderDetails.Items) {
                var sellerInfo = od.SellerInformation;
                var sodir = od.ToCustomerOrderDetailItemResponse()!;
                sodir.SellerInfo = sellerInfo!.ToSellerInfoResponse()!;
                customerOrderDetailItemResponses.Add(sodir);
            }
            var orderDetailsResponseList = new PagedList<CustomerSellerOrderItemResponse>(
                customerOrderDetailItemResponses,
                orderDetails.Page,
                orderDetails.PageSize,
                orderDetails.TotalItems
            );
            return Ok(orderDetailsResponseList);
        }
    }
}
