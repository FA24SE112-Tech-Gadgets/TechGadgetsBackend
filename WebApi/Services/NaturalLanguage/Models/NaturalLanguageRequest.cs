using Pgvector;

namespace WebApi.Services.NaturalLanguage.Models;

public class NaturalLanguageRequest
{
    public Vector? InputVector { get; set; }
    public List<string> Purposes { get; set; } = [];
    public List<string> Brands { get; set; } = [];
    public List<string> Categories { get; set; } = [];
    public bool IsFastCharge { get; set; }
    public bool IsGoodBatteryLife { get; set; }
    public float MinUsageTime { get; set; }
    public float MaxUsageTime { get; set; }
    public bool IsWideScreen { get; set; }
    public bool IsFoldable { get; set; }
    public float MinInch { get; set; }
    public float MaxInch { get; set; }
    public bool IsHighResolution { get; set; }
    public List<string> OperatingSystems { get; set; } = [];
}
