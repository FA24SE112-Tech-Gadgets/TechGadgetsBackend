namespace WebApi.Common.Settings;

public class ScrapeDataSettings
{
    public static readonly string Section = "ScrapeData";

    public string TGDD { get; set; } = default!;
    public string PhongVu { get; set; } = default!;
    public string FPTShop { get; set; } = default!;
}
