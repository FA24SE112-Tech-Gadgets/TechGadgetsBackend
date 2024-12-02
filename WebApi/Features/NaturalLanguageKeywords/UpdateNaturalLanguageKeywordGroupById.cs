using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.NaturalLanguageKeywords;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
[RequestValidation<Request>]
public class UpdateNaturalLanguageKeywordGroupById : ControllerBase
{
    public new class Request
    {
        public string? Name { get; set; }
        public NaturalLanguageKeywordGroupStatus? Status { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .When(r => r.Name != null)
                .WithMessage("Tên nhóm không được để chuỗi rỗng.");

        }
    }

    [HttpPatch("natural-language-keyword-groups/{groupId}")]
    [Tags("Natural Language Keywords")]
    [SwaggerOperation(
        Summary = "Manager Update Natural Language Keyword Group By Id"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid groupId, Request request, AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
                .Build();
        }

        var group = await context.NaturalLanguageKeywordGroups.FirstOrDefaultAsync(g => g.Id == groupId);

        if (group == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("naturalLanguageKeywordGroup", "Nhóm từ khoá không tồn tại.")
                .Build();
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            group.Name = request.Name;
        }

        if (request.Status != null)
        {
            group.Status = request.Status.Value;
        }

        if (context.Entry(group).State != EntityState.Unchanged)
        {
            group.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        return Ok("Cập nhật thành công");
    }
}
