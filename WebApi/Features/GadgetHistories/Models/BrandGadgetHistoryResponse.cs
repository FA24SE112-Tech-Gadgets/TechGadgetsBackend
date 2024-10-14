namespace WebApi.Features.GadgetHistories.Models;

public class BrandGadgetHistoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;
}
