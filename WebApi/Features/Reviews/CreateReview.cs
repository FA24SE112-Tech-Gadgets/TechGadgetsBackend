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

    [HttpPost("review/order-detail/{orderDetailId}/gadget/{gadgetId}")]
    [Tags("Reviews")]
    [SwaggerOperation(
        Summary = "Customer Create Review",
        Description = "API is for customer create review. Note:" +
                            "<br>&nbsp; - Rating là số nguyên từ 0 - 5." +
                            "<br>&nbsp; - Content không được để trống." +
                            "<br>&nbsp; - Mỗi gadget trong 1 đơn hàng chỉ được đánh 1 lần." +
                            "<br>&nbsp;     Ví dụ: Gadget A có trong 3 đơn thì customer có thể đánh giá Gadget A 3 lần" +
                            "<br>&nbsp; - Customer chỉ có thể đánh giá đơn của mình thôi" +
                            "<br>&nbsp; - Đánh giá gadget theo đơn. Tức là cần truyền gadgetId có trong orderDetailId đó." +
                            "<br>&nbsp; - Cho dù gadget Status = Inactive hay gadget Quantity = 0 thì đều review được."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromRoute] Guid orderDetailId, [FromRoute] Guid gadgetId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var orderDetail = await context.OrderDetails
            .Include(od => od.Order)
            .FirstOrDefaultAsync(od => od.Id == orderDetailId);
        if (orderDetail == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("orderDetail", "Không tìm thấy đơn này.")
            .Build();
        }

        if (currentUser!.Role == Role.Customer && orderDetail!.Order.CustomerId != currentUser!.Customer!.Id)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("orderDetail", "Người dùng không đủ thẩm quyền để truy cập đơn này.")
            .Build();
        }

        if (orderDetail.Status != OrderDetailStatus.Success)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("orderDetail", "Đơn này chưa hoàn thành hoặc đã hủy.")
            .Build();
        }

        bool isGadgetExist = await context.GadgetInformation
            .AnyAsync(gi => gi.GadgetId == gadgetId && gi.OrderDetailId == orderDetailId);

        if (!isGadgetExist)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("gadget", $"Sản phẩm {gadgetId} không có trong đơn {orderDetailId}. Hoặc sản phẩm không tồn tại.")
            .Build();
        }

        bool isAnyReview = await context.Reviews.AnyAsync(r => r.OrderDetailId == orderDetailId && r.GadgetId == gadgetId);

        if (isAnyReview)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_01)
            .AddReason("review", $"Sản phẩm {gadgetId} này trong đơn {orderDetailId} đã được đánh giá.")
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
            GadgetId = gadgetId,
            OrderDetailId = orderDetailId,
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
