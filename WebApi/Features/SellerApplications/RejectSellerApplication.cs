using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;

namespace WebApi.Features.SellerApplications;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
[RequestValidation<Request>]
public class RejectSellerApplication : ControllerBase
{
    public class Request
    {
        public string RejectReason { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(sp => sp.RejectReason)
                .NotEmpty()
                .WithMessage("Lý do từ chối không được để trống");
        }
    }

    [HttpPut("seller-applications/{sellerApplicationId}/reject")]
    [Tags("Seller Applications")]
    [SwaggerOperation(
        Summary = "Reject Seller Application",
        Description = "API is for Manager reject seller application."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromRoute] Guid sellerApplicationId, AppDbContext context, [FromBody] Request request)
    {
        var sellerApplication = await context.SellerApplications.FirstOrDefaultAsync(sa => sa.Id == sellerApplicationId) ?? throw TechGadgetException.NewBuilder()
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

        sellerApplication.Status = SellerApplicationStatus.Rejected;

        await context.SaveChangesAsync();

        return Ok();
    }
}
