namespace WebApi.Features.FavoriteGadgets.Models;

public class FavoriteGadgetResponse
{
    public DateTime CreatedAt { get; set; }
    public FavoriteGadgetItemResponse Gadget { get; set; } = default!;
}
