using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Services.Embedding;

namespace WebApi.Features.NaturalLanguages;

[ApiController]
public class TestingEndpoint : ControllerBase
{
    [Tags("Natural Language")]
    [HttpPost("natural-languages/test")]
    public async Task<IActionResult> Handler(AppDbContext context, EmbeddingService embeddingService)
    {
        var gadgets = await context.Gadgets
                                    .Include(g => g.SpecificationValues)
                                        .ThenInclude(sv => sv.SpecificationKey)
                                        .ThenInclude(sv => sv.SpecificationUnits)
                                    .Include(g => g.Brand)
                                    .Include(g => g.Category)
                                    .ToListAsync();


        List<string> list = [];

        foreach (var gadget in gadgets)
        {
            var gadgetAttribute = $"Tên sản phẩm: {gadget.Name}; Thể loại sản phẩm: {gadget.Category.Name}; Thương hiệu sản phẩm: {gadget.Brand.Name}; Thông số kỹ thuật: ";

            Dictionary<string, List<string>> specValueDictionary = [];
            foreach (var specValue in gadget.SpecificationValues)
            {
                if (!specValueDictionary.ContainsKey(specValue.SpecificationKey.Name))
                {
                    specValueDictionary[specValue.SpecificationKey.Name] = [];
                }
                if (specValue.SpecificationUnit != null)
                {
                    specValueDictionary[specValue.SpecificationKey.Name].Add($"{specValue.Value} {specValue.SpecificationUnit.Name}");
                }
                else
                {
                    specValueDictionary[specValue.SpecificationKey.Name].Add($"{specValue.Value}");
                }
            }

            foreach (var kvp in specValueDictionary)
            {
                var key = kvp.Key;
                var values = kvp.Value;
                gadgetAttribute += $"{key}: {string.Join(", ", values)}, ";
            }

            gadgetAttribute = gadgetAttribute.Length > 3000 ? gadgetAttribute[0..3000] : gadgetAttribute;

            list.Add(gadgetAttribute);
        }

        return Ok(new
        {
            Max = list.Max(s => s.Length),
            Value = list.OrderByDescending(s => s.Length).FirstOrDefault()
        });
    }
}
