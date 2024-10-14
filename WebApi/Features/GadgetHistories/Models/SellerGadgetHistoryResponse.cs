using WebApi.Data.Entities;

namespace WebApi.Features.GadgetHistories.Models;

public class SellerGadgetHistoryResponse
{
    public Guid Id { get; set; }
    public string? CompanyName { get; set; }
    public string ShopName { get; set; } = default!;
    public string ShopAddress { get; set; } = default!;
    public BusinessModel BusinessModel { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public UserGadgetHistoryResponse User { get; set; } = default!;
}
