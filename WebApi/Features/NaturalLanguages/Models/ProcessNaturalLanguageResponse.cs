using WebApi.Features.Gadgets.Models;
using WebApi.Features.Sellers.Models;

namespace WebApi.Features.NaturalLanguages.Models;

public class ProcessNaturalLanguageResponse
{
    public string Type { get; set; } = default!;
    public int Count { get; set; } = default!;
    public ICollection<SellerDetailResponse> Sellers { get; set; } = [];
    public ICollection<GadgetResponse> Gadgets { get; set; } = [];
}
