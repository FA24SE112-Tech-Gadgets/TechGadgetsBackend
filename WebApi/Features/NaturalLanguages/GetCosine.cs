using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Services.Embedding;

namespace WebApi.Features.NaturalLanguages;

[ApiController]
public class GetCosine : ControllerBase
{
    [Tags("Natural Language")]
    [HttpPost("natural-languages/get-cosine/{specificationValueId}")]
    public async Task<IActionResult> Handler(AppDbContext context, EmbeddingService embeddingService, Guid specificationValueId)
    {
        var embed = await embeddingService.GetEmbedding("Sạc nhanh");

        var item = await context.SpecificationValues
                        .Select(sv => new
                        {
                            sv.Id, // Include the Id in the projection if you need it later
                            Distance = 1 - sv.Vector.CosineDistance(embed)
                        })
                        .FirstOrDefaultAsync(sv => sv.Id == specificationValueId);

        return Ok(item);
    }
}
