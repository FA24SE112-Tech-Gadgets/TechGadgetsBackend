namespace WebApi.Features.Gadgets.Models;

public class GadgetImageResponse
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = default!;
}
