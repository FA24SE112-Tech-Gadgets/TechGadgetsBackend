namespace WebApi.Features.FavoriteGadgets.Models;

public class BrandFavoriteGadgetResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;
}
