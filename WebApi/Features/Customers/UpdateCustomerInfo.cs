﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.RegularExpressions;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
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
        public string? DateOfBirth { get; set; }
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
                .Matches("^[0-9]*$")
                .WithMessage("CCCD không được chứa chữ cái hoặc ký tự đặc biệt.")
                .When(r => r.CCCD != null); // Chỉ validate nếu CCCD được truyền

            RuleFor(r => r.Gender)
                .IsInEnum()
                .WithMessage("Giới tính không hợp lệ")
                .When(r => r.Gender != null); // Chỉ validate nếu Gender được truyền

            //RuleFor(r => r.DateOfBirth)
            //    .NotEmpty()
            //    .WithMessage("Ngày sinh không được để trống")
            //    .Must(BeAtLeast18YearsOld)
            //    .WithMessage("Phải trên 18 tuổi")
            //    .When(r => r.DateOfBirth != null); // Chỉ validate nếu DateOfBirth được truyền

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
        Description = "This API is for update customer info. Note:" +
                            "<br>&nbsp; - Truyền field nào update field đó." +
                            "<br>&nbsp; - CCCD chỉ yêu cầu đủ 12 kí tự và không chứa chữ cái hay ký tự đặc biệt." +
                            "<br>&nbsp; - Ngày sinh phải lớn hơn 18 tuổi. Format YYYY-MM-DD." +
                            "<br>&nbsp; - Số điện thoại có độ dài từ 10 - 11, không có chữ hay ký tự đặc biệt." +
                            "<br>&nbsp; - User bị Inactive thì không update info được."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromForm] Request request, AppDbContext context, [FromServices] GoogleStorageService storageService, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        // Lấy khách hàng từ database dựa trên user hiện tại
        var customer = currentUser!.Customer;

        if (customer == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("customer", "Không tìm thấy thông tin khách hàng.")
            .Build();
        }

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
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
                    .AddReason("avatarUrl", "Lỗi khi lưu ảnh đại diện")
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

        //if (request.DateOfBirth.HasValue)
        //{
        //    customer.DateOfBirth = DateTime.SpecifyKind(request.DateOfBirth.Value, DateTimeKind.Utc);
        //}

        if (!string.IsNullOrEmpty(request.DateOfBirth))
        {
            string pattern = @"^\d{4}/\d{2}/\d{2}$";

            if (Regex.IsMatch(request.DateOfBirth, pattern))
            {
                DateTime date = DateTime.ParseExact(request.DateOfBirth, "yyyy/MM/dd", null);

                var today = DateTime.Today;
                var age = today.Year - date.Year;
                if (date > today.AddYears(-age)) age--;
                if (age < 18)
                {
                    throw TechGadgetException.NewBuilder()
                           .WithCode(TechGadgetErrorCode.WEB_02)
                           .AddReason("dateOfBirth", "Phải trên 18 tuổi.")
                           .Build();
                }

                DateTime utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                customer.DateOfBirth = utcDate;
            }
            else
            {
                throw TechGadgetException.NewBuilder()
                    .WithCode(TechGadgetErrorCode.WEB_02)
                    .AddReason("dateOfBirth", "Ngày sinh không hợp lệ.")
                    .Build();
            }
        }

        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            customer.PhoneNumber = request.PhoneNumber;
        }

        if ((!string.IsNullOrEmpty(request.FullName) || !string.IsNullOrEmpty(request.Address) || !string.IsNullOrEmpty(request.PhoneNumber))
            && await context.CustomerInformation.AnyAsync(ci => ci.CustomerId == currentUser.Customer!.Id))
        {
            await context.CustomerInformation.AddAsync(new CustomerInformation
            {
                FullName = string.IsNullOrEmpty(request.FullName) ? customer.FullName : request.FullName,
                Address = string.IsNullOrEmpty(request.Address) ? customer.Address! : request.Address,
                PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) ? customer.PhoneNumber! : request.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                CustomerId = customer.Id,
            });
        }

        if (!string.IsNullOrEmpty(customer.PhoneNumber) && !string.IsNullOrEmpty(customer.Address)
            && !await context.CustomerInformation.AnyAsync(ci => ci.CustomerId == currentUser.Customer!.Id))
        {
            await context.CustomerInformation.AddAsync(new CustomerInformation
            {
                FullName = customer.FullName,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                CustomerId = customer.Id,
            });
        }

        // Lưu thay đổi vào database
        context.Customers.Update(customer);
        await context.SaveChangesAsync();

        return Ok("Cập nhật thành công");
    }
}
