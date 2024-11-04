using Microsoft.Extensions.Options;
using OpenAI.Embeddings;
using Pgvector;
using System.Text;
using System.Text.Json;
using WebApi.Common.Settings;

namespace WebApi.Services.Embedding;

public class EmbeddingResponse
{
    public float[] Embedding { get; set; } = [];
}

public class EmbeddingBatchResponse
{
    public float[][] Embeddings { get; set; } = [];
}

public class EmbeddingService(IHttpClientFactory httpClientFactory, IOptions<EmbeddingServerSettings> embeddingServerSettings, IOptions<OpenAIClientSettings> options)
{
    private readonly EmbeddingServerSettings _embeddingServerSettings = embeddingServerSettings.Value;
    private readonly OpenAIClientSettings _openAISettings = options.Value;

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

    public async Task<List<Vector>> GetEmbeddings(List<string> texts)
    {
        var client = httpClientFactory.CreateClient();
        var requestBody = new { texts };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        try
        {
            // Call the FastAPI embedding service
            var response = await client.PostAsync($"{_embeddingServerSettings.Url}/batch", jsonContent);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                var embeddingBatchResponse = JsonSerializer.Deserialize<EmbeddingBatchResponse>(resultContent,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                if (embeddingBatchResponse != null)
                {
                    // Convert the array of embeddings to List<Vector>
                    return embeddingBatchResponse.Embeddings.Select(emb => new Vector(emb)).ToList();
                }
                else
                {
                    throw new NullReferenceException("Embedding Batch Response is null");
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

    public async Task<Vector> GetEmbeddingOpenAI(string text, int dimensions = 1536)
    {
        EmbeddingClient client = new(_openAISettings.EmbeddingModel, _openAISettings.Key);
        OpenAIEmbedding embedding = await client.GenerateEmbeddingAsync(text, new EmbeddingGenerationOptions { Dimensions = dimensions });

        return new Vector(embedding.ToFloats());
    }

    public async Task<List<Vector>> GetEmbeddingsOpenAI(List<string> texts)
    {
        EmbeddingClient client = new(_openAISettings.EmbeddingModel, _openAISettings.Key);
        OpenAIEmbeddingCollection collection = await client.GenerateEmbeddingsAsync(texts);

        return collection.Select(e => new Vector(e.ToFloats())).ToList();
    }
}
