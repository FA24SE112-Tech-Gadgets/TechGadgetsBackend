namespace WebApi.Data.Entities;

public class Shop
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string WebsiteUrl { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;

    public ICollection<Gadget> Gadgets { get; set; } = [];
}
