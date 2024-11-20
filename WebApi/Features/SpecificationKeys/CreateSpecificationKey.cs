using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.SpecificationKeys;


[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
[RequestValidation<Request>]
public class CreateSpecificationKey : ControllerBase
{
    public new class Request
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<string> SpecificationUnits { get; set; } = [];
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId không được để trống");

            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("Tên SpecificationKey không được để trống");
        }
    }

    [HttpPost("specification-keys")]
    [Tags("Specification Keys")]
    [SwaggerOperation(
        Summary = "Manager Create Specification Key",
        Description = "API is for manager create specification key"
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Request request, AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
                .Build();
        }

        if (!await context.Categories.AnyAsync(b => b.Id == request.CategoryId))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("category", $"Thể loại {request.CategoryId} không tồn tại.")
                .Build();
        }

        if (await context.SpecificationKeys.AnyAsync(b => b.Name.ToLower() == request.Name.ToLower()))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_01)
                .AddReason("specificationKey", $"Tên specificationKey bị trùng.")
                .Build();
        }

        var newSpecificationKey = new SpecificationKey
        {
            Name = request.Name,
            CategoryId = request.CategoryId,
            SpecificationUnits = request.SpecificationUnits.Select(su => new SpecificationUnit
            {
                Name = su
            }).ToList()
        };

        context.SpecificationKeys.Add(newSpecificationKey);

        await context.SaveChangesAsync();

        return Created();
    }
}
