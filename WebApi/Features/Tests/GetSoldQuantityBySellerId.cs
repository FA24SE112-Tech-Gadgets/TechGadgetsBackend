using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Data.Entities;

namespace WebApi.Features.Tests;

[ApiController]
public class GetSoldQuantityBySellerId : ControllerBase
{
    [Tags("Tests")]
    [HttpGet("tests/sold-quantity/sellers/{sellerId}")]
    public async Task<IActionResult> Handler(Guid sellerId, AppDbContext context)
    {
        var res = await context.SellerOrderItems
                            .Where(soi => soi.SellerOrder.SellerId == sellerId && soi.SellerOrder.Status == SellerOrderStatus.Success)
                            .SumAsync(soi => soi.GadgetQuantity);

        return Ok(res);
    }
}
