using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;
using WebApi.Services.Storage;

namespace WebApi.Features.Brands;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
[RequestValidation<Request>]
public class CreateBrand : ControllerBase
{
    public new class Request
    {
        public IFormFile Logo { get; set; } = default!;
        public string Name { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("Tên thương hiệu không được để trống");

            RuleFor(r => r.Logo)
                .NotNull()
                .WithMessage("Logo không được để trống");
        }
    }

    [HttpPost("brands")]
    [Tags("Brands")]
    [SwaggerOperation(
        Summary = "Manager Create Brand",
        Description = "API is for manager create brand"
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromForm] Request request, AppDbContext context, CurrentUserService currentUserService, GoogleStorageService storageService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
                .Build();
        }

        if (await context.Brands.AnyAsync(b => b.Name.ToLower() == request.Name.ToLower()))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_01)
                .AddReason("brand", "Tên thương hiệu đã tồn tại.")
                .Build();
        }

        string? logoUrl = null;
        try
        {
            logoUrl = await storageService.UploadFileToCloudStorage(request.Logo, Guid.NewGuid().ToString());
        }
        catch (Exception)
        {
            if (logoUrl != null)
            {
                await storageService.DeleteFileFromCloudStorage(logoUrl);
            }
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WES_00)
                .AddReason("logoUrl", "Lỗi khi lưu logo")
                .Build();
        }

        var newBrand = new Brand
        {
            Name = request.Name,
            LogoUrl = logoUrl,
        };

        context.Brands.Add(newBrand);

        await context.SaveChangesAsync();

        return Created();
    }
}
