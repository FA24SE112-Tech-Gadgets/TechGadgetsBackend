namespace WebApi.Data.Entities;

public class GadgetHistory
{
    public Guid Id { get; set; }
    public Guid GadgetId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Customer Customer { get; set; } = default!;
    public Gadget Gadget { get; set; } = default!;
}
