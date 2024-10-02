namespace WebApi.Data.Entities;

public class GadgetDescription
{
    public Guid Id { get; set; }
    public Guid GadgetId { get; set; }
    public string Value { get; set; } = default!;
    public GadgetDescriptionType Type { get; set; }
    public int Index { get; set; }

    public Gadget Gadget { get; set; } = default!;
}

public enum GadgetDescriptionType
{
    Image, NormalText, BoldText
}
