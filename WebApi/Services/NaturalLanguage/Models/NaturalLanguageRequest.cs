using Pgvector;
using System.Text.Json.Serialization;

namespace WebApi.Services.NaturalLanguage.Models;

public class NaturalLanguageRequest
{
    [JsonIgnore]
    public Vector? InputVector { get; set; }
    public List<string> Purposes { get; set; } = [];
    public List<string> Brands { get; set; } = [];
    public List<string> Categories { get; set; } = [];
    public int MinPrice { get; set; }
    public int MaxPrice { get; set; }
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
    public List<string> StorageCapacitiesPhone { get; set; } = [];
    public List<string> StorageCapacitiesLaptop { get; set; } = [];
    public List<string> Rams { get; set; } = [];
    public List<string> Features { get; set; } = [];
    public List<string> Conditions { get; set; } = [];
    public List<string> Segmentations { get; set; } = [];
    public List<string> Locations { get; set; } = [];
    public List<string> Origins { get; set; } = [];
    public string MinReleaseDate { get; set; } = default!;
    public string MaxReleaseDate { get; set; } = default!;
    public List<string> Colors { get; set; } = [];
    public bool IsSmartPhone { get; set; }
    public bool IsSearchingSeller { get; set; }
    public bool IsBestGadget { get; set; }
    public bool IsHighRating { get; set; }
    public bool IsPositiveReview { get; set; }
    public bool IsEnergySaving { get; set; }
    public bool IsDiscounted { get; set; }
}
