using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Features.NaturalLanguages;

[ApiController]
public class GetSpecValueAsString : ControllerBase
{
    [Tags("Natural Language")]
    [HttpPost("natural-languages/compare/get-spec/{gadgetId}")]
    public async Task<IActionResult> Handler(AppDbContext context, Guid gadgetId)
    {
        var gadgets = await context.Gadgets
                        .Include(g => g.SpecificationValues)
                            .ThenInclude(sv => sv.SpecificationUnit)
                        .Include(g => g.SpecificationValues)
                            .ThenInclude(sv => sv.SpecificationKey)
                        .ToListAsync();

        string mx = "";
        foreach (var gadget in gadgets)
        {
            string s = gadget!.Name + " ";
            foreach (var specValue in gadget.SpecificationValues)
            {
                if (specValue.SpecificationUnit != null)
                {
                    s += specValue.SpecificationKey.Name + " " + specValue.Value + " " + specValue.SpecificationUnit.Name + " ";
                }
                else
                {
                    s += specValue.SpecificationKey.Name + " " + specValue.Value + " ";
                }
            }
            if (s.Length > mx.Length) mx = s;
        }

        return Ok(mx);
    }
}
