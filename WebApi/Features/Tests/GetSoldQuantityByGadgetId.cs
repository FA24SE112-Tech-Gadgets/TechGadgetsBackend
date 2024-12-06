using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Data.Entities;

namespace WebApi.Features.Tests;

[ApiController]
public class GetSoldQuantityByGadgetId : ControllerBase
{
    [Tags("Tests")]
    [HttpGet("tests/sold-quantity/{gadgetId}")]
    public async Task<IActionResult> Handler(Guid gadgetId, AppDbContext context)
    {
        var res = await context.SellerOrderItems
                            .Where(soi => soi.GadgetId == gadgetId && soi.SellerOrder.Status == SellerOrderStatus.Success)
                            .SumAsync(soi => soi.GadgetQuantity);

        return Ok(res);
    }
}
