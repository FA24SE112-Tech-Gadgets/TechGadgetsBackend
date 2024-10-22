namespace WebApi.Data.Entities;

public class CustomerInformation
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid OrderDetailId { get; set; }
    public string Address { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public Customer Customer { get; set; } = default!;
    public OrderDetail OrderDetail { get; set; } = default!;
}
