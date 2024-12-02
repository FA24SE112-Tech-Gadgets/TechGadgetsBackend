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
public class UpdateNaturalLanguageKeywordById : ControllerBase
{
    public new class Request
    {
        public string? Keyword { get; set; }
        public NaturalLanguageKeywordStatus? Status { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Keyword)
                .NotEmpty()
                .When(r => r.Keyword != null)
                .WithMessage("Từ khoá không được để chuỗi rỗng.");

        }
    }

    [HttpPatch("natural-language-keyword-groups/natural-language-keywords/{id}")]
    [Tags("Natural Language Keywords")]
    [SwaggerOperation(
        Summary = "Manager Update Natural Language Keyword By Id"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, Request request, AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
                .Build();
        }

        var keyword = await context.NaturalLanguageKeywords
                                    .Include(k => k.NaturalLanguageKeywordGroup)
                                    .FirstOrDefaultAsync(g => g.Id == id);

        if (keyword == null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("naturalLanguageKeyword", "Từ khoá không tồn tại.")
                .Build();
        }

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            keyword.Keyword = request.Keyword;
        }

        if (request.Status != null)
        {
            keyword.Status = request.Status.Value;
        }

        var now = DateTime.UtcNow;

        if (context.Entry(keyword).State != EntityState.Unchanged)
        {
            keyword.UpdatedAt = now;
            keyword.NaturalLanguageKeywordGroup.UpdatedAt = now;
        }

        await context.SaveChangesAsync();

        return Ok("Cập nhật thành công");
    }
}
