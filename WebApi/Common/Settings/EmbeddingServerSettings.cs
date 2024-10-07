namespace WebApi.Common.Settings;

public class EmbeddingServerSettings
{
    public static readonly string Section = "GoogleStorage";

    public string Url { get; set; } = default!;
}
