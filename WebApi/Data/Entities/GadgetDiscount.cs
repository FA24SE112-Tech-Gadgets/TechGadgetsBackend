namespace WebApi.Data.Entities;

public class GadgetDiscount
{
    public Guid Id { get; set; }
    public Guid GadgetId { get; set; }
    public int DiscountPercentage { get; set; }
    public DateTime ExpiredDate { get; set; }
    public GadgetDiscountStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Gadget Gadget { get; set; } = default!;
    public ICollection<SellerOrderItem> SellerOrderItems { get; set; } = [];
}

public enum GadgetDiscountStatus
{
    Active, Cancelled, Expired
}
