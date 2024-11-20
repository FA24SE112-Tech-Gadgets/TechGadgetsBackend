using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.SpecificationUnits;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
[RequestValidation<Request>]
public class CreateSpecificationUnit : ControllerBase
{
    public new class Request
    {
        public Guid SpecificationKeyId { get; set; }
        public string Name { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.SpecificationKeyId)
                .NotEmpty()
                .WithMessage("SpecificationKeyId không được để trống");

            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("Tên SpecificationUnit không được để trống");
        }
    }

    [HttpPost("specification-units")]
    [Tags("Specification Units")]
    [SwaggerOperation(
        Summary = "Manager Create Specification Unit",
        Description = "API is for manager create specification unit"
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

        if (!await context.SpecificationKeys.AnyAsync(b => b.Id == request.SpecificationKeyId))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("category", $"specificationKey {request.SpecificationKeyId} không tồn tại.")
                .Build();
        }

        if (await context.SpecificationUnits.AnyAsync(b => b.Name.ToLower() == request.Name.ToLower() && b.SpecificationKeyId == request.SpecificationKeyId))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_01)
                .AddReason("specificationUnit", $"Tên specificationUnit bị trùng.")
                .Build();
        }

        var newSpecificationUnit = new SpecificationUnit
        {
            Name = request.Name,
            SpecificationKeyId = request.SpecificationKeyId,
        };

        context.SpecificationUnits.Add(newSpecificationUnit);

        await context.SaveChangesAsync();

        return Created();
    }
}
