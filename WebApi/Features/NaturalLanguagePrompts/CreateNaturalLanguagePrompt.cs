using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.NaturalLanguagePrompts;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
[RequestValidation<Request>]
public class CreateNaturalLanguagePrompt : ControllerBase
{
    public new class Request
    {
        public string Prompt { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Prompt)
                .NotEmpty()
                .WithMessage("Prompt không được để trống.");
        }
    }

    [HttpPost("natural-language-prompts")]
    [Tags("Natural Language Prompts")]
    [SwaggerOperation(
        Summary = "Manager Create Natural Language Prompt"
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

        var now = DateTime.UtcNow;

        var prompt = new NaturalLanguagePrompt
        {
            CreatedAt = now,
            UpdatedAt = now,
            Prompt = request.Prompt
        };

        context.NaturalLanguagePrompts.Add(prompt);
        await context.SaveChangesAsync();

        return Created();
    }
}
