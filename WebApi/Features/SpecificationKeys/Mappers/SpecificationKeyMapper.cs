using WebApi.Data.Entities;
using WebApi.Features.SpecificationKeys.Models;

namespace WebApi.Features.SpecificationKeys.Mappers;

public static class SpecificationKeyMapper
{
    public static SpecificationKeyResponse? ToSpecificationKeyResponse(this SpecificationKey? specificationKey)
    {
        if (specificationKey != null)
        {
            return new SpecificationKeyResponse
            {
                Id = specificationKey.Id,
                CategoryId = specificationKey.CategoryId,
                Name = specificationKey.Name,
                SpecificationUnits = specificationKey.SpecificationUnits.Select(s =>
                    new SpecificationUnitResponse
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList(),
            };
        }
        return null;
    }
}
