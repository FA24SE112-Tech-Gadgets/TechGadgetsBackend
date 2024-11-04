using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Filters;
using WebApi.Services.Embedding;

namespace WebApi.Features.Tests;

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

    [Tags("Tests")]
    [HttpPost("tests/natural-languages/compare/embeddings")]
    public async Task<IActionResult> Handler(Request request, EmbeddingService embeddingService)
    {
        var vec1 = await embeddingService.GetEmbedding(request.Text1);
        var vec2 = await embeddingService.GetEmbedding(request.Text2);

        float[] v1 = vec1.ToArray();
        float[] v2 = vec2.ToArray();

        // Calculate distances
        float distance = CalculateL2Distance(v1, v2);
        float cosineSimilarity = CalculateCosineSimilarity(v1, v2);

        // Return both metrics (or modify as needed)
        return Ok(new
        {
            L2Distance = distance,
            L2DistanceDesc = "0: Indicates that the two vectors are identical (maximum similarity), Higher values indicate greater dissimilarity",
            CosineSimilarity = cosineSimilarity,
            CosineDesc = "1 is maximum similar, 0 is no similar, -1 is maximum dissimilar"
        });
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

    private static float CalculateCosineSimilarity(float[] v1, float[] v2)
    {
        if (v1.Length != v2.Length)
            throw new ArgumentException("Vectors must be of the same length.");

        float dotProduct = 0.0f;
        float magnitudeV1 = 0.0f;
        float magnitudeV2 = 0.0f;

        for (int i = 0; i < v1.Length; i++)
        {
            dotProduct += v1[i] * v2[i];
            magnitudeV1 += v1[i] * v1[i];
            magnitudeV2 += v2[i] * v2[i];
        }

        // Calculate the magnitudes
        magnitudeV1 = (float)Math.Sqrt(magnitudeV1);
        magnitudeV2 = (float)Math.Sqrt(magnitudeV2);

        // Handle the case where one of the vectors has zero magnitude
        if (magnitudeV1 == 0 || magnitudeV2 == 0)
            return 0; // or throw an exception

        // Return cosine similarity
        return dotProduct / (magnitudeV1 * magnitudeV2);
    }
}
