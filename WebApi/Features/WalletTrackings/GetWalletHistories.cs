using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.WalletTrackings.Mappers;
using WebApi.Features.WalletTrackings.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.WalletTrackings;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class GetWalletHistories : ControllerBase
{
    public new class Request : PagedRequest
    {
        public SortByDate SortByDate { get; set; }
        public List<WalletTrackingType>? Types { get; set; }
    }

    public enum SortByDate
    {
        DESC, ASC
    }

    [HttpGet("wallet-trackings")]
    [Tags("Wallet Trackings")]
    [SwaggerOperation(
        Summary = "Get List Of Wallet Trackings",
        Description = "This API is for get wallet transaction histories. Note: " +
                            "<br>&nbsp; - Type: Payment, Deposit, Refund, SellerTransfer." +
                            "<br>&nbsp; - Truyền vào danh sách các Types muốn lấy ra. Ví dụ: [Payment, Deposit, Refund]" +
                            "<br>&nbsp; - Không truyền Types suy ra get all types." +
                            "<br>&nbsp; - SortByDate: 'DESC' - Ngày gần nhất, 'ASC' - Ngày xa nhất. Nếu không truyền defaul: 'DESC'"
    )]
    [ProducesResponseType(typeof(PagedList<WalletTrackingItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(
        [FromQuery] Request request,
        AppDbContext context,
        [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var userWallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == currentUser!.Id);

        if (userWallet == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("wallets", "Không tìm thấy ví người dùng.")
            .Build();
        }

        var walletTrackingsQuery = context.WalletTrackings
            .Where(wt => wt.WalletId == userWallet.Id);

        // Kiểm tra nếu có danh sách Types được truyền vào
        if (request.Types != null && request.Types.Count > 0)
        {
            walletTrackingsQuery = walletTrackingsQuery
                .Where(wt => request.Types.Contains(wt.Type));
        }

        if (request.SortByDate == SortByDate.DESC)
        {
            // Thêm sắp xếp theo CreatedAt (giảm dần, gần nhất trước)
            walletTrackingsQuery = walletTrackingsQuery.OrderByDescending(wt => wt.CreatedAt);
        }
        else
        {
            walletTrackingsQuery = walletTrackingsQuery.OrderBy(wt => wt.CreatedAt);
        }

        // Thêm phần phân trang
        var walletTrackings = await walletTrackingsQuery
            .ToPagedListAsync(request);

        var walletTrackingsResponseList = new PagedList<WalletTrackingItemResponse>(
            walletTrackings.Items.Select(wt => wt.ToWalletTrackingItemResponse()!).ToList(),
            walletTrackings.Page,
            walletTrackings.PageSize,
            walletTrackings.TotalItems
        );

        return Ok(walletTrackingsResponseList);
    }
}
