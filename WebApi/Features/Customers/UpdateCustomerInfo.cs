using FluentValidation;
using Google.Apis.Storage.v1;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.SellerApplications.Models;
using WebApi.Services.Auth;
using WebApi.Services.Storage;

namespace WebApi.Features.Customers;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
[RequestValidation<Request>]
public class UpdateCustomerInfo : ControllerBase
{
    public new class Request
    {
        public string? FullName { get; set; } = default!;
        public IFormFile? Avatar { get; set; }
        public string? Address { get; set; }
        public string? CCCD { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.FullName)
                .NotEmpty()
                .WithMessage("Họ và tên không được để trống")
                .When(r => r.FullName != null); // Chỉ validate nếu FullName được truyền

            RuleFor(r => r.Address)
                .NotEmpty()
                .WithMessage("Địa chỉ không được để trống")
                .When(r => r.Address != null); // Chỉ validate nếu Address được truyền

            RuleFor(r => r.CCCD)
                .NotEmpty()
                .WithMessage("CCCD không được để trống")
                .Length(12)
                .WithMessage("CCCD phải đủ 12 ký tự")
                .When(r => r.CCCD != null); // Chỉ validate nếu CCCD được truyền

            RuleFor(r => r.Gender)
                .IsInEnum()
                .WithMessage("Giới tính không hợp lệ")
                .When(r => r.Gender != null); // Chỉ validate nếu Gender được truyền

            RuleFor(r => r.DateOfBirth)
                .NotEmpty()
                .WithMessage("Ngày sinh không được để trống")
                .Must(BeAtLeast18YearsOld)
                .WithMessage("Phải trên 18 tuổi")
                .When(r => r.DateOfBirth != null); // Chỉ validate nếu DateOfBirth được truyền

            RuleFor(r => r.PhoneNumber)
                .NotEmpty()
                .WithMessage("Số điện thoại không được để trống")
                .Length(10, 11)
                .WithMessage("Số điện thoại phải có độ dài 10 hoặc 11 số")
                .Matches("^[0-9]*$")
                .WithMessage("Số điện thoại không được chứa chữ cái hoặc ký tự đặc biệt.")
                .When(r => r.PhoneNumber != null); // Chỉ validate nếu PhoneNumber được truyền
        }
        private bool BeAtLeast18YearsOld(DateTime? dateOfBirth)
        {
            if (dateOfBirth == null) return false;
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Value.Year;
            if (dateOfBirth.Value > today.AddYears(-age)) age--;
            return age >= 18;
        }
    }

    [HttpPatch("customer")]
    [Tags("Customers")]
    [SwaggerOperation(
        Summary = "Update Customer Info",
        Description = "This API is for update customer info." +
                            "<br>&nbsp; - Update which field is provided"
    )]
    [ProducesResponseType(typeof(SellerApplicationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromForm] Request request, AppDbContext context, [FromServices] GoogleStorageService storageService, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        // Lấy khách hàng từ database dựa trên user hiện tại
        var customer = await context.Customers.FindAsync(currentUser!.Customer!.Id);
        if (customer == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("customer", "Không tìm thấy thông tin khách hàng.")
            .Build();
        }

        // Cập nhật các trường nếu chúng được truyền
        if (!string.IsNullOrEmpty(request.FullName))
        {
            customer.FullName = request.FullName;
        }

        if (request.Avatar != null)
        {
            string? avatarUrl = null;
            try
            {
                avatarUrl = await storageService.UploadFileToCloudStorage(request.Avatar!, Guid.NewGuid().ToString());
                customer.AvatarUrl = avatarUrl;
            }
            catch (Exception)
            {
                if (avatarUrl != null)
                {
                    await storageService.DeleteFileFromCloudStorage(avatarUrl);
                }
                throw TechGadgetException.NewBuilder()
                    .WithCode(TechGadgetErrorCode.WES_00)
                    .AddReason("businessRegistrationCertificate", "Lỗi khi lưu ảnh đại diện")
                    .Build();
            }
        }

        if (!string.IsNullOrEmpty(request.Address))
        {
            customer.Address = request.Address;
        }

        if (!string.IsNullOrEmpty(request.CCCD))
        {
            customer.CCCD = request.CCCD;
        }

        if (request.Gender.HasValue)
        {
            customer.Gender = request.Gender;
        }

        if (request.DateOfBirth.HasValue)
        {
            customer.DateOfBirth = DateTime.SpecifyKind(request.DateOfBirth.Value, DateTimeKind.Utc);
        }

        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            customer.PhoneNumber = request.PhoneNumber;
        }

        // Lưu thay đổi vào database
        context.Customers.Update(customer);
        await context.SaveChangesAsync();

        return Ok();
    }
}
