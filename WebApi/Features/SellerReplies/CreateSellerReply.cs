using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.SellerReplies;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Seller)]
[RequestValidation<Request>]
public class CreateSellerReply : ControllerBase
{
    public new class Request
    {
        public string Content { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Content)
                .NotEmpty()
                .WithMessage("Nội dung phản hồi không được để trống");
        }
    }

    [HttpPost("seller-repy/review/{reviewId}")]
    [Tags("Seller Replies")]
    [SwaggerOperation(
        Summary = "Seller Create Reply",
        Description = "API is for seller create reply. Note:" +
                            "<br>&nbsp; - Content không được để trống." +
                            "<br>&nbsp; - Mỗi review chỉ được phản hồi 1 lần." +
                            "<br>&nbsp; - Seller chỉ có thể phản hồi sản phẩm của mình thôi" +
                            "<br>&nbsp; - Cho dù gadget Status = Inactive hay gadget Quantity = 0 thì đều reply được." +
                            "<br>&nbsp; - Không thể reply những review bị status Inactive." +
                            "<br>&nbsp; - Không thể reply những review đã quá 10 phút." +
                            "<br>&nbsp; - User bị Inactive thì không thể phản hồi đành giá được."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromRoute] Guid reviewId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        if (currentUser!.Seller is null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("seller", "Seller chưa được kích hoạt")
                .Build();
        }

        var review = await context.Reviews
            .Include(r => r.SellerOrderItem)
                .ThenInclude(soi => soi.SellerOrder)
            .FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("review", "Không tìm thấy đánh giá này.")
            .Build();
        }

        if (review.Status == ReviewStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("review", "Đánh giá này đã bị chặn.")
            .Build();
        }

        if (review.SellerOrderItem.SellerOrder.SellerId != currentUser!.Seller!.Id)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("reply", "Người dùng không đủ thẩm quyền để phản hồi đánh giá này.")
            .Build();
        }

        if (review.CreatedAt <= DateTime.UtcNow.AddMinutes(-10))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("reply", "Đã quá thời gian phản hồi (10 phút).")
            .Build();
        }

        var createdAt = DateTime.UtcNow;
        SellerReply sellerReply = new SellerReply()
        {
            ReviewId = reviewId,
            SellerId = currentUser.Seller!.Id,
            Content = request.Content,
            Status = SellerReplyStatus.Active,
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
        }!;

        await context.SellerReplies.AddAsync(sellerReply);
        await context.SaveChangesAsync();

        return Created();
    }
}
