using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Data;
using WebApi.Features.TestScrape.Mappers;
using WebApi.Features.Users.Models;

namespace WebApi.Features.TestScrape;

[ApiController]
public class GetScrapeGadget : ControllerBase
{
    [HttpPost("test/gadgets")]
    [Tags("Test")]
    [SwaggerOperation(Summary = "Get Current User", Description = "This API is for getting the current authenticated user")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Handler([FromForm] string categoryName, [FromServices] AppDbContext context)
    {
        try
        {
            var gadgetsByCategory = await context.Gadgets
                .Where(g => g.Category.Name == categoryName) // Lọc theo tên Category
                .Include(g => g.Specifications) // Bao gồm Specifications
                    .ThenInclude(s => s.SpecificationKeys) // Bao gồm SpecificationKeys của mỗi Specification
                        .ThenInclude(sk => sk.SpecificationValues) // Bao gồm SpecificationValues của mỗi SpecificationKey
                .Include(g => g.SpecificationKeys)
                    .ThenInclude(sk => sk.SpecificationValues)
                .Include(g => g.GadgetImages)
                .Include(g => g.GadgetDescriptions)
                .AsSplitQuery() // Sử dụng SplitQuery để xử lý việc load nhiều collection
                .ToListAsync(); // Lấy toàn bộ danh sách gadget

            return Ok(gadgetsByCategory.ToGadgetListResponse());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"loi ne: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
}
