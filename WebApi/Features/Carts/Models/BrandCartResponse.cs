namespace WebApi.Features.Carts.Models;

public class BrandCartResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;
}
