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
public class CreateCriteriaByGroupId : ControllerBase
{
    public new class Request
    {
        public CriteriaType Type { get; set; }
        public Guid? SpecificationKeyId { get; set; }
        public string? Contains { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public ICollection<Guid> Categories { get; set; } = [];
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(c => c.Categories)
                    .NotEmpty()
                    .When(c => c.Type != CriteriaType.Specification)
                    .WithMessage("Danh sách thể loại áp dụng không được để trống khi Type khác Specification.");

            RuleFor(x => x.Categories)
                    .Must(c => c.Distinct().Count() == c.Count)
                    .When(c => c.Type != CriteriaType.Specification)
                    .WithMessage("Danh sách thể loại không được có giá trị trùng lặp.");

            RuleFor(c => c.Categories)
                .Empty()
                .When(c => c.Type == CriteriaType.Specification)
                .WithMessage("Danh sách thể loại áp dụng phải để trống với Type Specification.");

            RuleFor(c => c.Contains)
                .NotEmpty()
                .When(c => c.Type == CriteriaType.Name)
                .WithMessage("Contains không được để trống khi Type là Name.");

            RuleFor(c => c.Contains)
                .NotEmpty()
                .When(c => c.Type == CriteriaType.Description)
                .WithMessage("Contains không được để trống khi Type là Description.");

            RuleFor(c => c.Contains)
                .NotEmpty()
                .When(c => c.Type == CriteriaType.Condition)
                .WithMessage("Contains không được để trống khi Type là Condition.");

            RuleFor(c => c.Contains)
                .NotEmpty()
                .When(c => c.Type == CriteriaType.Specification)
                .WithMessage("Contains không được để trống khi Type là Specification.");

            RuleFor(c => c.SpecificationKeyId)
                .NotNull()
                .When(c => c.Type == CriteriaType.Specification)
                .WithMessage("SpecificationKeyId không được để trống khi Type là Specification");

            RuleFor(c => c.MinPrice)
                .NotNull()
                .When(c => c.Type == CriteriaType.Price)
                .WithMessage("MinPrice không được để trống khi Type là Price.")
                .GreaterThanOrEqualTo(0)
                .When(c => c.Type == CriteriaType.Price)
                .WithMessage("MinPrice phải lớn hơn hoặc bằng 0 khi Type là Price.")
                .LessThanOrEqualTo(150_000_000)
                .When(c => c.Type == CriteriaType.Price)
                .WithMessage("MinPrice phải nhỏ hơn hoặc bằng 150 triệu khi Type là Price.");

            RuleFor(c => c.MaxPrice)
                .NotNull()
                .When(c => c.Type == CriteriaType.Price)
                .WithMessage("MaxPrice không được để trống khi Type là Price.")
                .GreaterThanOrEqualTo(0)
                .When(c => c.Type == CriteriaType.Price)
                .WithMessage("MaxPrice phải lớn hơn hoặc bằng 0 khi Type là Price.")
                .LessThanOrEqualTo(150_000_000)
                .When(c => c.Type == CriteriaType.Price)
                .WithMessage("MaxPrice phải nhỏ hơn hoặc bằng 150 triệu khi Type là Price.");

            RuleFor(c => c)
                .Must(c => c.MaxPrice >= c.MinPrice)
                .When(c => c.Type == CriteriaType.Price)
                .WithMessage("MinPrice không được nhỏ hơn MaxPrice.");
        }
    }

    [HttpPost("natural-language-keyword-groups/{groupId}/criteria")]
    [Tags("Natural Language Keywords")]
    [SwaggerOperation(
        Summary = "Manager Create Criteria By GroupId",
        Description = """
        `Type`: Specification, Name, Description, Condition, Price

        - `Specification`: phải cung cấp specificationKeyId
        - `Name, Description, Condition`: phải cung cấp contains
        - `Price`: phải cung cấp minPrice, maxPrice. (giá trị trong khoảng 0 - 150 triệu)

        Nếu type là `Specification` thì `categories` để mảng rỗng, còn lại các type khác phải chứa ít nhất 1 item
        """
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

        if (request.SpecificationKeyId.HasValue && request.Type == CriteriaType.Specification &&
            !await context.SpecificationKeys.AnyAsync(s => s.Id == request.SpecificationKeyId.Value))
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("specificationKey", $"SpecificationKeyId không tồn tại: {request.SpecificationKeyId}")
                .Build();
        }

        List<Guid> notExistingCategoryIds = [];

        foreach (var categoryId in request.Categories)
        {
            var exists = await context.Categories
                              .AnyAsync(c => c.Id == categoryId);

            if (!exists)
            {
                notExistingCategoryIds.Add(categoryId);
            }
        }

        if (notExistingCategoryIds.Any())
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("categories", $"Những CategoryId không tồn tại: {string.Join(", ", notExistingCategoryIds)}")
                .Build();
        }

        var now = DateTime.UtcNow;

        var criteria = new Criteria
        {
            MaxPrice = request.MaxPrice,
            SpecificationKeyId = request.SpecificationKeyId,
            Contains = request.Contains,
            MinPrice = request.MinPrice,
            Type = request.Type,
            UpdatedAt = now,
            CreatedAt = now,
            NaturalLanguageKeywordGroupId = groupId,
            CriteriaCategories = request.Categories.Select(c => new CriteriaCategory
            {
                CategoryId = c
            }).ToList()
        };

        group.UpdatedAt = now;

        context.Criteria.Add(criteria);
        await context.SaveChangesAsync();

        return Created();
    }
}

