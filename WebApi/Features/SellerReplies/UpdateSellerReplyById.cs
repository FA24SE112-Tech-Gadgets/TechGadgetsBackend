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
public class UpdateSellerReplyById : ControllerBase
{
    public new class Request
    {
        public string? Content { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Content)
                .NotEmpty()
                .WithMessage("Nội dung phản hồi không được để trống")
                .When(r => r.Content != null); // Chỉ validate nếu Content được truyền
        }
    }

    [HttpPatch("seller-reply/{sellerReplyId}")]
    [Tags("Seller Replies")]
    [SwaggerOperation(
        Summary = "Seller Update Reply By SellerReplyId",
        Description = "API is for seller update reply by sellerReplyId. Note:" +
                            "<br>&nbsp; - Nội dung không được để trống." +
                            "<br>&nbsp; - Truyền field nào update field đó." +
                            "<br>&nbsp; - Chỉ được thay đổi phản hồi 1 lần." +
                            "<br>&nbsp; - Chỉ được cập nhật phản hồi của bản thân." +
                            "<br>&nbsp; - User bị Inactive thì không cập nhật phản hồi được."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromRoute] Guid sellerReplyId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
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

        var sellerReply = await context.SellerReplies
            .FirstOrDefaultAsync(soi => soi.Id == sellerReplyId);
        if (sellerReply == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerReply", "Không tìm thấy phản hồi này.")
            .Build();
        }

        if (sellerReply.SellerId != currentUser!.Seller!.Id)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("sellerReply", "Người dùng không đủ thẩm quyền để cập nhật phản hồi này.")
            .Build();
        }

        if (sellerReply.Status != SellerReplyStatus.Active)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("sellerReply", "Phản hồi này đã bị chặn.")
            .Build();
        }

        if (sellerReply.CreatedAt <= DateTime.UtcNow.AddMinutes(-10))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("sellerReply", "Đã quá thời gian sửa phản hồi (10 phút).")
            .Build();
        }

        bool isUpdated = sellerReply.CreatedAt != sellerReply.UpdatedAt;

        if (isUpdated)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_01)
            .AddReason("sellerReply", $"Sản phẩm này đã sửa phản hồi rồi.")
            .Build();
        }

        if (request.Content != null)
        {
            sellerReply.Content = request.Content;
            sellerReply.UpdatedAt = DateTime.UtcNow;
        }

        context.SellerReplies.Update(sellerReply);
        await context.SaveChangesAsync();

        return Ok("Cập nhật thành công");
    }
}
