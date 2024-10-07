namespace WebApi.Common.Settings;

public class EmbeddingServerSettings
{
    public static readonly string Section = "EmbeddingServer";

    public string Url { get; set; } = default!;
}
