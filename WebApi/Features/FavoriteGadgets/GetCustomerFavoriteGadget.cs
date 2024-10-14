using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.FavoriteGadgets.Mappers;
using WebApi.Features.FavoriteGadgets.Models;
using WebApi.Features.SellerApplications.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.FavoriteGadgets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class GetCustomerFavoriteGadget : ControllerBase
{
    public new class Request : PagedRequest
    {
        public SortByDate SortByDate { get; set; }
    }

    public enum SortByDate
    {
        DESC, ASC
    }

    [HttpGet("favorite-gadgets")]
    [Tags("Favorite Gadgets")]
    [SwaggerOperation(
        Summary = "Get List Of Customer Favorite Gadgets",
        Description = "This API is for get list of customer favorite gadgets." +
                            "<br>&nbsp; - SortByDate: 'DESC' - Ngày gần nhất, 'ASC' - Ngày xa nhất. Nếu không truyền defaul: 'DESC'"
    )]
    [ProducesResponseType(typeof(SellerApplicationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var query = context.FavoriteGadgets
            .Include(fg => fg.Gadget)
                .ThenInclude(g => g.Seller)
                    .ThenInclude(s => s.User)
            .Include(fg => fg.Gadget)
                .ThenInclude(g => g.Brand)
            .Include(fg => fg.Gadget)
                .ThenInclude(g => g.Category)
            .Where(fg => fg.CustomerId == currentUser!.Customer!.Id)
            .AsQueryable();

        if (request.SortByDate == SortByDate.DESC)
        {
            // Thêm sắp xếp theo CreatedAt (giảm dần, gần nhất trước)
            query = query.OrderByDescending(sa => sa.CreatedAt);
        }
        else
        {
            query = query.OrderBy(sa => sa.CreatedAt);
        }

        var favoriteGadgets = await query
            .ToPagedListAsync(request)
            ?? throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("favoriteGadgets", "Không tìm thấy sản phẩm yêu thích.")
            .Build();

        var favoriteGadgetResponseList = new PagedList<FavoriteGadgetResponse>(
            favoriteGadgets.Items.Select(fg => fg.ToFavoriteGadgetResponse()!).ToList(),
            favoriteGadgets.Page,
            favoriteGadgets.PageSize,
            favoriteGadgets.TotalItems
        );

        return Ok(favoriteGadgetResponseList);
    }
}
