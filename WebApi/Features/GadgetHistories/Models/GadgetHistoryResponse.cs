using WebApi.Data.Entities;

namespace WebApi.Features.GadgetHistories.Models;

public class GadgetHistoryResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public GadgetHistoryItemResponse Gadget { get; set; } = default!;
}
