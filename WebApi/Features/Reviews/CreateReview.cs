using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Reviews;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
[RequestValidation<Request>]
public class CreateReview : ControllerBase
{
    public new class Request
    {
        public int Rating { get; set; }
        public string Content { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Content)
                .NotEmpty()
                .WithMessage("Nội dung đánh giá không được để trống");

            RuleFor(r => r.Rating)
                .InclusiveBetween(0, 5)
                .WithMessage("Đánh giá phải nằm trong khoảng từ 0 đến 5");
        }
    }

    [HttpPost("review/seller-order-item/{sellerOrderItemId}")]
    [Tags("Reviews")]
    [SwaggerOperation(
        Summary = "Customer Create Review",
        Description = "API is for customer create review. Note:" +
                            "<br>&nbsp; - Rating là số nguyên từ 0 - 5." +
                            "<br>&nbsp; - Content không được để trống." +
                            "<br>&nbsp; - Mỗi gadget trong 1 đơn hàng chỉ được đánh 1 lần." +
                            "<br>&nbsp;     Ví dụ: Gadget A có trong 3 đơn thì customer có thể đánh giá Gadget A 3 lần" +
                            "<br>&nbsp; - Customer chỉ có thể đánh giá đơn của mình thôi" +
                            "<br>&nbsp; - Đánh giá gadget theo đơn. Tức là chỉ cần truyền sellerOrderItemId của gadget trong đơn đó." +
                            "<br>&nbsp; - Cho dù gadget Status = Inactive hay gadget Quantity = 0 thì đều review được." +
                            "<br>&nbsp; - Không thể review những item đã quá 10 phút." +
                            "<br>&nbsp; - User bị Inactive thì không đánh giá được."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromRoute] Guid sellerOrderItemId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        var sellerOrderItem = await context.SellerOrderItems
            .Include(soi => soi.SellerOrder)
                .ThenInclude(so => so.Order)
            .FirstOrDefaultAsync(soi => soi.Id == sellerOrderItemId);
        if (sellerOrderItem == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerOrderItem", "Không tìm thấy sản phẩm này.")
            .Build();
        }

        if (sellerOrderItem.SellerOrder.Order.CustomerId != currentUser!.Customer!.Id)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("sellerOrder", "Người dùng không đủ thẩm quyền để đánh giá sản phẩm này.")
            .Build();
        }

        if (sellerOrderItem.SellerOrder.Status != SellerOrderStatus.Success)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerOrder", "Đơn này chưa hoàn thành hoặc đã hủy.")
            .Build();
        }

        if (sellerOrderItem.SellerOrder.CreatedAt <= DateTime.UtcNow.AddMinutes(-10))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("review", "Đã quá thời gian đánh giá (10 phút).")
            .Build();
        }

        bool isAnyReview = sellerOrderItem.Review != null;

        if (isAnyReview)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_01)
            .AddReason("review", $"Sản phẩm này đã được đánh giá rồi.")
            .Build();
        }

        bool isPositive = false;
        if (request.Rating >= 3)
        {
            isPositive = true;
        }

        var createdAt = DateTime.UtcNow;
        Review review = new Review()
        {
            SellerOrderItemId = sellerOrderItemId,
            CustomerId = currentUser.Customer!.Id,
            Rating = request.Rating,
            Content = request.Content,
            IsPositive = isPositive,
            Status = ReviewStatus.Active,
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
        }!;

        await context.Reviews.AddAsync(review);
        await context.SaveChangesAsync();

        return Created();
    }
}
