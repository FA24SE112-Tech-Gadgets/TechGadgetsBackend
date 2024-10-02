namespace WebApi.Data.Entities;

public class BillingMail
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public string Mail { get; set; } = default!;

    public Seller Seller { get; set; } = default!;
}
