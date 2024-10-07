using Microsoft.Extensions.Options;
using Pgvector;
using System.Text;
using System.Text.Json;
using WebApi.Common.Settings;

namespace WebApi.Services.Embedding;

public class EmbeddingResponse
{
    public float[] Embedding { get; set; } = [];
}

public class EmbeddingService(IHttpClientFactory httpClientFactory, IOptions<EmbeddingServerSettings> embeddingServerSettings)
{
    private readonly EmbeddingServerSettings _embeddingServerSettings = embeddingServerSettings.Value;

    public async Task<Vector> GetEmbedding(string text)
    {
        var client = httpClientFactory.CreateClient();

        var requestBody = new { text };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        try
        {
            // Call the FastAPI embedding service
            var response = await client.PostAsync($"{_embeddingServerSettings.Url}", jsonContent);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();

                // Deserialize the response content
                var embeddingResponse = JsonSerializer.Deserialize<EmbeddingResponse>(resultContent,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                if (embeddingResponse != null)
                {
                    return new Vector(embeddingResponse.Embedding);
                }
                else
                {
                    throw new NullReferenceException("Embedding Response is null");
                }
            }
            else
            {
                throw new InvalidOperationException("Error calling the embedding service. Status code: " + response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error calling the embedding service.", ex);
        }
    }
}
