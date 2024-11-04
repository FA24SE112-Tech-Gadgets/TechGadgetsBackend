using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.SellerApplications.Mappers;
using WebApi.Services.Auth;
using WebApi.Services.Embedding;

namespace WebApi.Features.SellerApplications;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class ApproveSellerApplication : ControllerBase
{
    [HttpPut("seller-applications/{sellerApplicationId}/approve")]
    [Tags("Seller Applications")]
    [SwaggerOperation(
        Summary = "Approve Seller Application",
        Description = "API is for Manager approve seller application. Note:" +
                            "<br>&nbsp; - User bị Inactive thì không thể duyệt đơn được."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid sellerApplicationId, AppDbContext context, [FromServices] EmbeddingService embeddingService, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        var sellerApplication = await context.SellerApplications
            .Include(sa => sa.BillingMailApplications)
            .FirstOrDefaultAsync(sa => sa.Id == sellerApplicationId) ?? throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerApplication", "Không tìm thấy đơn này.")
            .Build();

        if (sellerApplication.Status == SellerApplicationStatus.Rejected)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("sellerApplication", "Đơn này đã bị từ chối trước đó.")
                .Build();
        }

        var sellerExist = await context.Sellers.AnyAsync(s => s.UserId == sellerApplication.UserId);
        if (sellerExist)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_01)
                .AddReason("sellerApplication", "Đơn này đã được duyệt trước đó.")
                .Build();
        }

        sellerApplication.Status = SellerApplicationStatus.Approved;

        Seller seller = sellerApplication.ToSellerCreate()!;
        var addressVector = await embeddingService.GetEmbedding(seller.ShopAddress.Trim());
        seller.AddressVector = addressVector;

        if (seller == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WES_00)
                .AddReason("sellerApplication", "Có lỗi xảy ra trong quá trình duyệt đơn.")
                .Build();
        }
        await context.Sellers.AddAsync(seller);
        await context.SaveChangesAsync();

        return Ok("Duyệt đơn thành công");
    }
}
