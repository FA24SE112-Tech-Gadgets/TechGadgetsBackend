using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Admins;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Admin)]
[RequestValidation<Request>]
public class UpdateAdminInfo : ControllerBase
{
    public new class Request
    {
        public string? FullName { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.FullName)
                .NotEmpty()
                .WithMessage("Tên không được để trống")
                .When(r => r.FullName != null); // Chỉ validate nếu FullName được truyền
        }
    }

    [HttpPatch("admin")]
    [Tags("Admins")]
    [SwaggerOperation(
        Summary = "Update Admin Info",
        Description = "This API is for update admin info. Note:" +
                            "<br>&nbsp; - Truyền field nào update field đó." +
                            "<br>&nbsp; - Chỉ có FullName có thể được update." +
                            "<br>&nbsp; - Không thể cập nhật info nếu User bị Inactive."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromForm] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        // Lấy khách hàng từ database dựa trên user hiện tại
        var admin = await context.Admins.FindAsync(currentUser!.Admin!.Id);
        if (admin == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("admin", "Không tìm thấy thông tin quản trị viên này.")
            .Build();
        }

        if (admin.User.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        // Cập nhật các trường nếu chúng được truyền
        if (!string.IsNullOrEmpty(request.FullName))
        {
            admin.FullName = request.FullName;

            // Lưu thay đổi vào database
            context.Admins.Update(admin);
            await context.SaveChangesAsync();

            return Ok("Cập nhật thông tin thành công");
        }

        return Ok("Không có trường dữ liệu nào được cập nhật.");
    }
}
