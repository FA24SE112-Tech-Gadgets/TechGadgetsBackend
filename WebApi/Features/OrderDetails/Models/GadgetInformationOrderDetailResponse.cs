namespace WebApi.Features.OrderDetails.Models;

public class GadgetInformationOrderDetailResponse
{
    public Guid Id { get; set; }
    public Guid OrderDetailId { get; set; }
    public string GadgetThumbnailUrl { get; set; } = default!;
    public string GadgetName { get; set; } = default!;
    public int GadgetPrice { get; set; } = default!;
    public int GadgetQuantity { get; set; } = default!;
    public Guid GadgetId { get; set; }
}
