namespace WebApi.Common.Settings;

public class AesEncryptionSettings
{
    public static readonly string Section = "AesEncryption";

    public string Key { get; set; } = default!;
    public string IV { get; set; } = default!;
}
