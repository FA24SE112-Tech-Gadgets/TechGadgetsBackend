using WebApi.Data.Entities;
using WebApi.Features.Categories.Mappers;
using WebApi.Features.Gadgets.Models;
using WebApi.Features.NaturalLanguageKeywords.Models;

namespace WebApi.Features.NaturalLanguageKeywords.Mappers;

public static class NaturalLanguageKeywordGroupMapper
{
    public static NaturalLanguageKeywordGroupResponse? ToNaturalLanguageKeywordGroupResponse(this NaturalLanguageKeywordGroup? group)
    {
        if (group != null)
        {
            return new NaturalLanguageKeywordGroupResponse
            {
                Id = group.Id,
                Name = group.Name,
                NaturalLanguageKeywords = group.NaturalLanguageKeywords.Select(k => k.ToNaturalLanguageKeywordResponse()!).ToList(),
                Criteria = group.Criteria.Select(c => c.ToCriteriaResponse()!).ToList(),
                CreatedAt = group.CreatedAt,
                UpdatedAt = group.UpdatedAt,
                Status = group.Status,
            };
        }
        return null;
    }

    public static CriteriaResponse? ToCriteriaResponse(this Criteria? criteria)
    {
        if (criteria != null)
        {
            return new CriteriaResponse
            {
                Id = criteria.Id,
                Type = criteria.Type,
                SpecificationKey = criteria.SpecificationKey is not null ? new SpecificationKeyResponse
                {
                    Id = criteria.SpecificationKey.Id,
                    Name = criteria.SpecificationKey.Name
                } : null,
                Contains = criteria.Contains,
                MinPrice = criteria.MinPrice,
                MaxPrice = criteria.MaxPrice,
                CreatedAt = criteria.CreatedAt,
                UpdatedAt = criteria.UpdatedAt,
                Categories = criteria.Categories.Select(c => c.ToCategoryResponse()!).ToList(),
            };
        }
        return null;
    }

    public static NaturalLanguageKeywordResponse? ToNaturalLanguageKeywordResponse(this NaturalLanguageKeyword? keyword)
    {
        if (keyword != null)
        {
            return new NaturalLanguageKeywordResponse
            {
                Id = keyword.Id,
                Keyword = keyword.Keyword,
                Status = keyword.Status,
                CreatedAt = keyword.CreatedAt,
                UpdatedAt = keyword.UpdatedAt,
            };
        }
        return null;
    }
}
