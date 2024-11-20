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
public class DeleteSpecificationUnit : ControllerBase
{
    [HttpDelete("specification-units/{id}")]
    [Tags("Specification Units")]
    [SwaggerOperation(
        Summary = "Manager Delete Specification Unit"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
                .Build();
        }

        if (!await context.SpecificationUnits.AnyAsync(b => b.Id == id))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("specificationUnit", $"Không tìm thấy specificationUnit {id}.")
                .Build();
        }

        if (await context.SpecificationValues.AnyAsync(g => g.SpecificationUnitId == id))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("specificationUnit", $"specificationUnit {id} đang được tham chiếu tới, không thể xoá.")
                .Build();
        }

        await context.SpecificationUnits
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync();

        return Ok("Xóa specificationUnit thành công");
    }
}
