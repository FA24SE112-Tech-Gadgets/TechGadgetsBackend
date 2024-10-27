namespace WebApi.Data.Entities;

public class CustomerInformation
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Address { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public Customer Customer { get; set; } = default!;
    public ICollection<SellerOrder> SellerOrders { get; set; } = [];
}
