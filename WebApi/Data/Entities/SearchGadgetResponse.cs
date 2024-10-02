namespace WebApi.Data.Entities;

public class SearchGadgetResponse
{
    public Guid Id { get; set; }
    public Guid SearchHistoryResponseId { get; set; }
    public Guid GadgetId { get; set; }

    public Gadget Gadget { get; set; } = default!;
    public SearchHistoryResponse SearchHistoryResponse { get; set; } = default!;
}
