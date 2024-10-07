using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.SellerApplications.Mappers;
using WebApi.Services.Auth;
using WebApi.Services.Storage;

namespace WebApi.Features.SellerApplications;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class ApproveSellerApplication : ControllerBase
{
    [HttpPut("seller-applications/{sellerApplicationId}")]
    [Tags("Seller Applications")]
    [SwaggerOperation(
        Summary = "Approve Seller Application",
        Description = "API is for approve seller application."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid sellerApplicationId, AppDbContext context, GoogleStorageService storageService, [FromServices] CurrentUserService currentUserService)
    {
        var sellerApplication = await context.SellerApplications.FirstOrDefaultAsync(sa => sa.Id == sellerApplicationId) ?? throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerApplication", "Không tìm thấy đơn này.")
            .Build();

        sellerApplication.Status = SellerApplicationStatus.Approved;

        Seller seller = sellerApplication.ToSeller()!;
        if (seller == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WES_00)
                .AddReason("sellerApplication", "Có lỗi xảy ra trong quá trình duyệt đơn.")
                .Build();
        }
        await context.Sellers.AddAsync(seller);
        await context.SaveChangesAsync();

        return Ok();
    }
}
