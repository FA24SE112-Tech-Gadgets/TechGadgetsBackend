using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.GadgetDiscounts;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Seller)]
[RequestValidation<Request>]
public class CreateGadgetDiscountByGadgetId : ControllerBase
{
    public new class Request
    {
        public int DiscountPercentage { get; set; }
        public DateTime DiscountExpiredDate { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.DiscountPercentage)
                .NotEmpty()
                .WithMessage("Phần trăm giảm giá không được để trống")
                .GreaterThan(0)
                .WithMessage("Phần trăm giảm giá phải lớn hơn 0")
                .LessThanOrEqualTo(90)
                .WithMessage("Phần trăm giảm giá phải nhỏ hơn hoặc bằng 90"); // Chỉ validate nếu DiscountPercentage được truyền

            RuleFor(r => r.DiscountExpiredDate)
                .NotEmpty()
                .WithMessage("Ngày hết hạn không được để trống")
                .Must(date => date.ToUniversalTime() > DateTime.UtcNow)
                .WithMessage("Ngày hết hạn phải lớn hơn thời gian hiện tại"); // Chỉ validate nếu ShopName được truyền
        }
    }

    [HttpPost("gadget-discount/{gadgetId}")]
    [Tags("Gadget Discounts")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [SwaggerOperation(Summary = "Create Gadget Discount By GadgetId",
        Description = """
        This API is for creating gadget discount by gadgetId
        - Dùng API để thêm giảm giá cho gadget (Update cũng bằng API này)
        - Update tương đương với Inactive discount hiện tại và tạo mới discount khác
        - DiscountPercentage:
            - Phần trăm giảm giá không được để trống
            - Phần trăm giảm giá phải lớn hơn 0
            - Phần trăm giảm giá phải nhỏ hơn hoặc bằng 90

        - DiscountExpiredDate:
            - Ngày hết hạn phải lớn hơn thời gian hiện tại
        """
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromForm] Request request,
        AppDbContext context, [FromRoute] Guid gadgetId, CurrentUserService currentUserService)
    {
        var user = await currentUserService.GetCurrentUser();

        if (user!.Seller is null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("seller", "Seller chưa được kích hoạt")
                .Build();
        }

        if (user.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("seller", "Tài khoản Seller đã bị khoá")
                .Build();
        }

        var currGadget = await context.Gadgets
            .FirstOrDefaultAsync(g => g.Id == gadgetId);

        if (currGadget == null) {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("gadget", "Sản phẩm không tồn tại")
                .Build();
        }

        if (currGadget.SellerId != user.Seller.Id)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("seller", "Người dùng không đủ thẩm quyền để truy cập sản phẩm này.")
            .Build();
        }
        var existGadgetDiscount = await context.GadgetDiscounts
            .FirstOrDefaultAsync(gd => gd.GadgetId == gadgetId && gd.Status == GadgetDiscountStatus.Active);
        if (existGadgetDiscount != null)
        {
            existGadgetDiscount.Status = GadgetDiscountStatus.Cancelled;
        }

        GadgetDiscount gadgetDiscount = new GadgetDiscount
        {
            GadgetId = gadgetId,
            DiscountPercentage = request.DiscountPercentage,
            ExpiredDate = request.DiscountExpiredDate,
            Status = GadgetDiscountStatus.Active,
            CreatedAt = DateTime.UtcNow,
        }!;

        context.GadgetDiscounts.Add(gadgetDiscount);
        await context.SaveChangesAsync();

        return Ok("Thêm giảm giá thành công");
    }
}
