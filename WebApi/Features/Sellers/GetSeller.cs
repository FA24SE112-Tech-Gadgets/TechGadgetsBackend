using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Data;
using WebApi.Features.Sellers.Mappers;
using WebApi.Features.Sellers.Models;

namespace WebApi.Features.Sellers;

[ApiController]
public class GetSeller : ControllerBase
{
    [HttpGet("sellers/{sellerId}")]
    [Tags("Sellers")]
    [SwaggerOperation(Summary = "Get Seller Information For Customers",
        Description = """
        This API is for retrieving seller by id
        """
    )]
    [ProducesResponseType(typeof(SellerResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Handler(Guid sellerId, AppDbContext context)
    {
        if (!await context.Sellers.AnyAsync(c => c.Id == sellerId))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("seller", "seller không tồn tại")
            .Build();
        }

        var seller = await context.Sellers
                    .FirstOrDefaultAsync(s => s.Id == sellerId);

        return Ok(seller!.ToSellerResponse());
    }
}
