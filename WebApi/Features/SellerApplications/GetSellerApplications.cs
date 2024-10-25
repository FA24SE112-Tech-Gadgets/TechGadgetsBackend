using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.SellerApplications.Mappers;
using WebApi.Features.SellerApplications.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.SellerApplications;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager, Role.Seller)]
public class GetSellerApplications : ControllerBase
{
    public new class Request : PagedRequest
    {
        public SortByDate SortByDate { get; set; }
        public SellerApplicationStatus? Status { get; set; }
    }

    public enum SortByDate
    {
        DESC, ASC
    }

    [HttpGet("seller-applications")]
    [Tags("Seller Applications")]
    [SwaggerOperation(
        Summary = "Get List Of Seller Applications",
        Description = "API is for get list of seller applications." +
                            "<br>&nbsp; - SortByDate: 'DESC' - Ngày gần nhất, 'ASC' - Ngày xa nhất. Nếu không truyền defaul: 'DESC'" +
                            "<br>&nbsp; - Status: 'Pending', 'Approved', 'Rejected'." +
                            "<br>&nbsp; - Manager thì có thể xem all còn Seller chỉ có thể xem của bản thân thôi."
    )]
    [ProducesResponseType(typeof(SellerApplicationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var query = context.SellerApplications.AsQueryable();

        if (currentUser!.Role == Role.Seller)
        {
            query = query.Where(sa => sa.UserId == currentUser!.Id);
        }

        if (request.Status != null)
        {
            query = query.Where(sa => sa.Status == request.Status);
        }

        if (request.SortByDate == SortByDate.DESC)
        {
            // Thêm sắp xếp theo CreatedAt (giảm dần, gần nhất trước)
            query = query.OrderByDescending(sa => sa.CreatedAt);
        } else
        {
            query = query.OrderBy(sa => sa.CreatedAt);
        }

        var sellerApplications = await query
            .ToPagedListAsync(request);

        var sellerApplicationsResponseList = new PagedList<SellerApplicationItemResponse>(
            sellerApplications.Items.Select(sa => sa.ToSellerApplicationItemResponse()!).ToList(),
            sellerApplications.Page,
            sellerApplications.PageSize,
            sellerApplications.TotalItems
        );

        return Ok(sellerApplicationsResponseList);
    }
}
