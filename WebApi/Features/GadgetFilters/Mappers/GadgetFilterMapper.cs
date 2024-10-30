using WebApi.Data.Entities;
using WebApi.Features.GadgetFilters.Models;

namespace WebApi.Features.GadgetFilters.Mappers;

public static class GadgetFilterMapper
{
    public static GadgetFilterResponse? ToGadgetFilterResponse(this GadgetFilter? gadgetFilter)
    {
        if (gadgetFilter != null)
        {
            return new GadgetFilterResponse
            {
                GadgetFilterId = gadgetFilter.Id,
                Value = gadgetFilter.Value + (gadgetFilter.SpecificationUnit != null ? (" " + gadgetFilter.SpecificationUnit.Name) : "")
            };
        }
        return null;
    }

    public static List<GadgetFilterResponse>? ToListGadgetFilterResponse(this ICollection<GadgetFilter> gadgetFilters)
    {
        if (gadgetFilters != null)
        {
            return gadgetFilters
            .Select(gadgetFilter => gadgetFilter.ToGadgetFilterResponse())
            .ToList()!;
        }
        return null;
    }
}
