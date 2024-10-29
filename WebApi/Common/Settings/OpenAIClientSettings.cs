namespace WebApi.Common.Settings;

public class OpenAIClientSettings
{
    public static readonly string Section = "OpenAIClient";

    public string StructuredOutputModel { get; set; } = default!;
    public string EmbeddingModel { get; set; } = default!;
    public string Key { get; set; } = default!;
}
