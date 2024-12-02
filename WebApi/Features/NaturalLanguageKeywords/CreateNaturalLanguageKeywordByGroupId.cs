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
public class CreateNaturalLanguageKeywordByGroupId : ControllerBase
{
    public new class Request
    {
        public string Keyword { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Keyword)
                .NotEmpty()
                .WithMessage("Từ khoá không được để trống.");
        }
    }

    [HttpPost("natural-language-keyword-groups/{groupId}/natural-language-keywords")]
    [Tags("Natural Language Keywords")]
    [SwaggerOperation(
        Summary = "Manager Create Natural Language Keyword By GroupId"
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
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

        if (await context.NaturalLanguageKeywords.AnyAsync(k => k.Keyword.ToLower() == request.Keyword.ToLower()))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_01)
                .AddReason("naturalLanguageKeyword", "Từ khoá đã tồn tại.")
                .Build();
        }

        var now = DateTime.UtcNow;

        var keyword = new NaturalLanguageKeyword
        {
            Keyword = request.Keyword,
            NaturalLanguageKeywordGroupId = groupId,
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordStatus.Active,
        };

        group.UpdatedAt = now;

        context.NaturalLanguageKeywords.Add(keyword);
        await context.SaveChangesAsync();

        return Created();
    }
}

