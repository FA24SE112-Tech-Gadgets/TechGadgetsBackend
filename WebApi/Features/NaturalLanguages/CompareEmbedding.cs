using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Filters;
using WebApi.Services.Embedding;

namespace WebApi.Features.NaturalLanguages;

[ApiController]
[RequestValidation<Request>]
public class CompareEmbedding : ControllerBase
{
    public new class Request
    {
        public string Text1 { get; set; } = default!;
        public string Text2 { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Text1)
                .NotEmpty().WithMessage("Input không được để trống");

            RuleFor(r => r.Text2)
                .NotEmpty().WithMessage("Input không được để trống");
        }
    }

    [Tags("Natural Language")]
    [HttpPost("natural-languages/compare/embeddings")]
    public async Task<IActionResult> Handler(Request request, EmbeddingService embeddingService)
    {
        var vec1 = await embeddingService.GetEmbedding(request.Text1);
        var vec2 = await embeddingService.GetEmbedding(request.Text2);

        float[] v1 = vec1.ToArray();
        float[] v2 = vec2.ToArray();
        float distance = CalculateL2Distance(v1, v2);

        return Ok(distance);
    }

    private static float CalculateL2Distance(float[] v1, float[] v2)
    {
        if (v1.Length != v2.Length)
            throw new ArgumentException("Vectors must be of the same length.");

        float sum = 0.0f;

        for (int i = 0; i < v1.Length; i++)
        {
            float difference = v1[i] - v2[i];
            sum += difference * difference;
        }

        return (float)Math.Sqrt(sum);
    }
}
