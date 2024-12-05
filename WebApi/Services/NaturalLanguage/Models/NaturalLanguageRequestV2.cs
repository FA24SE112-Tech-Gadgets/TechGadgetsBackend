namespace WebApi.Services.NaturalLanguage.Models;

public class NaturalLanguageRequestV2
{
    public List<string> Keywords { get; set; } = [];
    public List<string> Brands { get; set; } = [];
    public List<string> Categories { get; set; } = [];
    public int MinPrice { get; set; }
    public int MaxPrice { get; set; }
    public List<string> OperatingSystems { get; set; } = [];
    public List<string> StorageCapacitiesPhone { get; set; } = [];
    public List<string> StorageCapacitiesLaptop { get; set; } = [];
    public List<string> Rams { get; set; } = [];
    public List<string> Locations { get; set; } = [];
    public List<string> Origins { get; set; } = [];
    public List<string> ReleaseDate { get; set; } = [];
    public List<string> Colors { get; set; } = [];
    public bool IsSearchingSeller { get; set; }
    public bool IsBestGadget { get; set; }
    public bool IsHighRating { get; set; }
    public bool IsPositiveReview { get; set; }
    public bool IsDiscounted { get; set; }
    public bool IsBestSeller { get; set; }
    public bool IsAvailable { get; set; }
    public string SellerName { get; set; } = default!;
}
