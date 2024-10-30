using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;
using WebApi.Services.Embedding;

namespace WebApi.Features.Sellers;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Seller)]
[RequestValidation<Request>]
public class UpdateSellerInfo : ControllerBase
{
    public new class Request
    {
        public string? CompanyName { get; set; } = default!;
        public string? ShopName { get; set; }
        public string? ShopAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.CompanyName)
                .NotEmpty()
                .WithMessage("Tên công ty không được để trống")
                .When(r => r.CompanyName != null); // Chỉ validate nếu CompanyName được truyền

            RuleFor(r => r.ShopName)
                .NotEmpty()
                .WithMessage("Tên cửa hàng không được để trống")
                .When(r => r.ShopName != null); // Chỉ validate nếu ShopName được truyền

            RuleFor(r => r.ShopAddress)
                .NotEmpty()
                .WithMessage("Địa chỉ không được để trống")
                .When(r => r.ShopAddress != null); // Chỉ validate nếu ShopAddress được truyền

            RuleFor(r => r.PhoneNumber)
                .NotEmpty()
                .WithMessage("Số điện thoại không được để trống")
                .Length(10, 11)
                .WithMessage("Số điện thoại phải có độ dài 10 hoặc 11 số")
                .Matches("^[0-9]*$")
                .WithMessage("Số điện thoại không được chứa chữ cái hoặc ký tự đặc biệt.")
                .When(r => r.PhoneNumber != null); // Chỉ validate nếu PhoneNumber được truyền
        }
    }

    [HttpPatch("seller")]
    [Tags("Sellers")]
    [SwaggerOperation(
        Summary = "Update Seller Info",
        Description = "This API is for update seller info. Note:" +
                            "<br>&nbsp; - Truyền field nào update field đó." +
                            "<br>&nbsp; - Chỉ được update: CompanyName, ShopName, ShopAddress và PhoneNumber." +
                            "<br>&nbsp; - Số điện thoại có độ dài từ 10 - 11, không có chữ hay ký tự đặc biệt."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromForm] Request request, AppDbContext context, [FromServices] EmbeddingService embeddingService, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Seller is null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("seller", "Seller chưa được kích hoạt")
                .Build();
        }

        // Lấy khách hàng từ database dựa trên user hiện tại
        var seller = await context.Sellers.FindAsync(currentUser!.Seller!.Id);
        if (seller == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("seller", "Không tìm thấy thông tin cửa hàng.")
            .Build();
        }

        // Cập nhật các trường nếu chúng được truyền
        if (!string.IsNullOrEmpty(request.CompanyName))
        {
            seller.CompanyName = request.CompanyName;
        }

        if (!string.IsNullOrEmpty(request.ShopAddress))
        {
            seller.ShopAddress = request.ShopAddress;
            seller.AddressVector = await embeddingService.GetEmbedding(request.ShopAddress.Trim());
        }

        if (!string.IsNullOrEmpty(request.ShopName))
        {
            seller.ShopName = request.ShopName;
        }

        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            seller.PhoneNumber = request.PhoneNumber;
        }

        // Lưu thay đổi vào database
        context.Sellers.Update(seller);
        await context.SaveChangesAsync();

        return Ok();
    }
}
