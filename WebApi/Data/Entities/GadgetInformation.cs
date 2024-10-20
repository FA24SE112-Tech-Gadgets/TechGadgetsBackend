namespace WebApi.Data.Entities;

public class GadgetInformation
{
    public Guid Id { get; set; }
    public Guid OrderDetailId { get; set; }
    public string GadgetName { get; set; } = default!;
    public string GadgetThumbnailUrl { get; set; } = default!;
    public int GadgetPrice { get; set; } = default!;
    public int GadgetQuantity { get; set; } = default!;
    public Guid GadgetId { get; set; }

    public OrderDetail OrderDetail { get; set; } = default!;
    public Gadget Gadget { get; set; } = default!;
}
