﻿using FluentValidation;
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
                .NotEmpty().WithMessage("Tên nhóm không được để trống.");

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

                c.RuleFor(x => x.Categories)
                    .Must(c => c.Distinct().Count() == c.Count)
                    .When(c => c.Type != CriteriaType.Specification)
                    .WithMessage("Danh sách thể loại không được có giá trị trùng lặp.");

                c.RuleFor(c => c.Categories)
                    .Empty()
                    .When(c => c.Type == CriteriaType.Specification)
                    .WithMessage("Danh sách thể loại áp dụng phải để trống với Type Specification.");

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
                    .WithMessage("MinPrice phải lớn hơn hoặc bằng 0 khi Type là Price.")
                    .LessThanOrEqualTo(150_000_000)
                    .When(c => c.Type == CriteriaType.Price)
                    .WithMessage("MinPrice phải nhỏ hơn hoặc bằng 150 triệu khi Type là Price.");

                c.RuleFor(c => c.MaxPrice)
                    .NotNull()
                    .When(c => c.Type == CriteriaType.Price)
                    .WithMessage("MaxPrice không được để trống khi Type là Price.")
                    .GreaterThanOrEqualTo(0)
                    .When(c => c.Type == CriteriaType.Price)
                    .WithMessage("MaxPrice phải lớn hơn hoặc bằng 0 khi Type là Price.")
                    .LessThanOrEqualTo(150_000_000)
                    .When(c => c.Type == CriteriaType.Price)
                    .WithMessage("MaxPrice phải nhỏ hơn hoặc bằng 150 triệu khi Type là Price.");

                c.RuleFor(c => c)
                    .Must(c => c.MaxPrice >= c.MinPrice)
                    .When(c => c.Type == CriteriaType.Price)
                    .WithMessage("MinPrice không được nhỏ hơn MaxPrice.");

            });
        }
    }

    [HttpPost("natural-language-keyword-groups")]
    [Tags("Natural Language Keywords")]
    [SwaggerOperation(
        Summary = "Manager Create Natural Language Keyword Group",
        Description = """
        ### Keywords:
        mảng `keywords` phải chứa ít nhất 1 item

        ### Criteria:
        mảng `criteria` phải chứa ít nhất 1 item

        *Các item trong mảng:* 

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

        var existingKeywords = await context.NaturalLanguageKeywords
                    .Where(k => request.Keywords.Any(rk => rk.ToLower() == k.Keyword.ToLower()))
                    .Select(k => k.Keyword.ToLower())
                    .ToListAsync();

        if (existingKeywords.Any())
        {
            throw TechGadgetException.NewBuilder()
                    .WithCode(TechGadgetErrorCode.WEB_01)
                    .AddReason("keywords", $"Những từ khoá đã tồn tại: {string.Join(", ", existingKeywords)}")
                    .Build();
        }

        List<Guid> notExistingSpecificationKeys = [];

        foreach (var criteria in request.Criteria)
        {
            if (criteria.SpecificationKeyId.HasValue && criteria.Type == CriteriaType.Specification)
            {
                var exists = await context.SpecificationKeys
                                  .AnyAsync(spec => spec.Id == criteria.SpecificationKeyId.Value);

                if (!exists)
                {
                    notExistingSpecificationKeys.Add(criteria.SpecificationKeyId.Value);
                }
            }
        }

        if (notExistingSpecificationKeys.Any())
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_00)
                .AddReason("specificationKeys", $"Những SpecificationKeyId không tồn tại: {string.Join(", ", notExistingSpecificationKeys)}")
                .Build();
        }

        List<Guid> notExistingCategoryIds = [];

        foreach (var criteria in request.Criteria)
        {
            foreach (var categoryId in criteria.Categories)
            {
                var exists = await context.Categories
                                  .AnyAsync(c => c.Id == categoryId);

                if (!exists)
                {
                    notExistingCategoryIds.Add(categoryId);
                }
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
        var group = new NaturalLanguageKeywordGroup
        {
            Name = request.Name,
            Criteria = request.Criteria.Select(c => new Criteria
            {
                Type = c.Type,
                SpecificationKeyId = c.SpecificationKeyId,
                Contains = c.Contains,
                MinPrice = c.MinPrice,
                MaxPrice = c.MaxPrice,
                CreatedAt = now,
                UpdatedAt = now,
                CriteriaCategories = c.Categories.Select(c => new CriteriaCategory
                {
                    CategoryId = c
                }).ToList()
            }).ToList(),
            NaturalLanguageKeywords = request.Keywords.Select(k => new NaturalLanguageKeyword
            {
                Keyword = k,
                Status = NaturalLanguageKeywordStatus.Active,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList(),
            Status = NaturalLanguageKeywordGroupStatus.Active,
            CreatedAt = now,
            UpdatedAt = now
        };

        context.NaturalLanguageKeywordGroups.Add(group);
        await context.SaveChangesAsync();

        return Created();
    }
}
