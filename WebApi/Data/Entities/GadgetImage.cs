namespace WebApi.Data.Entities;

public class GadgetImage
{
    public Guid Id { get; set; }
    public Guid GadgetId { get; set; }
    public string ImageUrl { get; set; } = default!;

    public Gadget Gadget { get; set; } = default!;
}
