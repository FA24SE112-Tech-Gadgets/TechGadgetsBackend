using FluentValidation;
using Microsoft.AspNetCore.Mvc;
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
public class CreateNaturalLanguageKeywordGroup : ControllerBase
{
    public class CriteriaRequest
    {
        public CriteriaType Type { get; set; }
        public Guid? SpecificationKeyId { get; set; }
        public string? Contains { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public ICollection<Guid> Categories { get; set; } = [];
    }

    public new class Request
    {
        public string Name { get; set; } = default!;
        public ICollection<string> Keywords { get; set; } = [];
        public ICollection<CriteriaRequest> Criteria { get; set; } = [];
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Tên nhóm không được để trống");

            RuleFor(r => r.Keywords)
                .NotEmpty().WithMessage("Danh sách từ khoá không được để trống.")
                .ForEach(keyword => keyword.NotEmpty().WithMessage("Từ khoá không được để trống."))
                .Must(r => r.Distinct(StringComparer.OrdinalIgnoreCase).Count() == r.Count())
                .WithMessage("Danh sách từ khoá không được có giá trị trùng lặp."); ;

            RuleFor(r => r.Criteria)
                .NotEmpty().WithMessage("Danh sách tiêu chí không được để trống.");

            RuleForEach(r => r.Criteria).ChildRules(c =>
            {
                c.RuleFor(c => c.Categories)
                    .NotEmpty()
                    .When(c => c.Type != CriteriaType.Specification)
                    .WithMessage("Danh sách thể loại áp dụng không được để trống khi Type khác Specification.");

                c.RuleFor(c => c.Contains)
                    .NotEmpty()
                    .When(c => c.Type == CriteriaType.Name)
                    .WithMessage("Contains không được để trống khi Type là Name.");

                c.RuleFor(c => c.Contains)
                    .NotEmpty()
                    .When(c => c.Type == CriteriaType.Description)
                    .WithMessage("Contains không được để trống khi Type là Description.");

                c.RuleFor(c => c.Contains)
                    .NotEmpty()
                    .When(c => c.Type == CriteriaType.Condition)
                    .WithMessage("Contains không được để trống khi Type là Condition.");

                c.RuleFor(c => c.Contains)
                    .NotEmpty()
                    .When(c => c.Type == CriteriaType.Specification)
                    .WithMessage("Contains không được để trống khi Type là Specification.");

                c.RuleFor(c => c.SpecificationKeyId)
                    .NotNull()
                    .When(c => c.Type == CriteriaType.Specification)
                    .WithMessage("SpecificationKeyId không được để trống khi Type là Specification");

                c.RuleFor(c => c.MinPrice)
                    .NotNull()
                    .When(c => c.Type == CriteriaType.Price)
                    .WithMessage("MinPrice không được để trống khi Type là Price.")
                    .GreaterThanOrEqualTo(0)
                    .When(c => c.Type == CriteriaType.Price)
                    .WithMessage("MinPrice phải lớn hơn hoặc bằng 0 khi Type là Price.");

                c.RuleFor(c => c.MaxPrice)
                    .NotNull()
                    .When(c => c.Type == CriteriaType.Price)
                    .WithMessage("MaxPrice không được để trống khi Type là Price.")
                    .LessThanOrEqualTo(150_000_000)
                    .When(c => c.Type == CriteriaType.Price)
                    .WithMessage("MaxPrice phải nhỏ hơn hoặc bằng 150 triệu khi Type là Price.");

            });
        }
    }

    [HttpPost("natural-language-keyword-groups")]
    [Tags("Natural Language Keywords")]
    [SwaggerOperation(
        Summary = "Manager Create Natural Language Keyword Group"
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

        var existingKeywords = await context.
                    .Where(k => request.Keywords.Contains(k.Name))
                    .Select(k => k.Name)
                    .ToListAsync();

        return Created();
    }
}
