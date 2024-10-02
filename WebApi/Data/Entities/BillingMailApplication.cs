namespace WebApi.Data.Entities;

public class BillingMailApplication
{
    public Guid Id { get; set; }
    public Guid SellerApplicationId { get; set; }
    public string Mail { get; set; } = default!;

    public SellerApplication SellerApplication { get; set; } = default!;
}
