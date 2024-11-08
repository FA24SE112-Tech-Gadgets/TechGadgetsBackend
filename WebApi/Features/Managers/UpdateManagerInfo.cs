using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Managers;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
[RequestValidation<Request>]
public class UpdateManagerInfo : ControllerBase
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

    [HttpPatch("manager")]
    [Tags("Managers")]
    [SwaggerOperation(
        Summary = "Update Manager Info",
        Description = "This API is for update manager info. Note:" +
                            "<br>&nbsp; - Truyền field nào update field đó." +
                            "<br>&nbsp; - Chỉ có FullName có thể được update." +
                            "<br>&nbsp; - User bị Inactive thì không cập nhật thông tin được."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromForm] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        // Lấy khách hàng từ database dựa trên user hiện tại
        var manager = await context.Managers.FindAsync(currentUser!.Manager!.Id);
        if (manager == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("manager", "Không tìm thấy thông tin nhân viên quản lý này.")
            .Build();
        }

        // Cập nhật các trường nếu chúng được truyền
        if (!string.IsNullOrEmpty(request.FullName))
        {
            manager.FullName = request.FullName;
        }

        // Lưu thay đổi vào database
        context.Managers.Update(manager);
        await context.SaveChangesAsync();

        return Ok("Cập nhật thành công");
    }
}
