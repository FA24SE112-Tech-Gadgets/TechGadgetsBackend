using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Features.TestScrape.Mappers;
using WebApi.Features.TestScrape.Models;
using WebApi.Features.Users.Models;

namespace WebApi.Features.TestScrape;

[ApiController]
public class GetScrapeGadget : ControllerBase
{
    public new class Request : PagedRequest
    {
        public string CategoryName { get; set; } = string.Empty;
        public string ShopName { get; set; } = string.Empty;
    }

    [HttpPost("test/gadgets")]
    [Tags("Test")]
    [SwaggerOperation(
        Summary = "Get Current User",
        Description = "API is for get gadgets test. Note:" +
                            "<br>&nbsp; - Category name: 'Điện thoại', 'Laptop', 'Thiết bị âm thanh'." +
                            "<br>&nbsp; - Shop name: 'Thế Giới Di Động', 'Phong Vũ', 'FPT Shop'." +
                            "<br>&nbsp; - Đối với 'Phong Vũ' thì chưa có scrape data."
    )]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Handler([FromQuery] Request request, [FromServices] AppDbContext context)
    {
        try
        {
            var gadgetsByCategory = await context.Gadgets
                .Include(g => g.Specifications) // Bao gồm Specifications
                    .ThenInclude(s => s.SpecificationKeys) // Bao gồm SpecificationKeys của mỗi Specification
                        .ThenInclude(sk => sk.SpecificationValues) // Bao gồm SpecificationValues của mỗi SpecificationKey
                .Include(g => g.SpecificationKeys)
                    .ThenInclude(sk => sk.SpecificationValues)
                .Include(g => g.GadgetImages)
                .Include(g => g.GadgetDescriptions)
                .Include(g => g.Shop)
                .Include(g => g.Category)
                .Where(g => (g.Category.Name == request.CategoryName && g.Shop!.Name == request.ShopName)) // Lọc theo tên Category
                .AsSplitQuery() // Sử dụng SplitQuery để xử lý việc load nhiều collection
                .ToPagedListAsync(request); // Lấy toàn bộ danh sách gadget

            // Chuyển đổi sang GadgetResponseTest
            var gadgetResponseList = new PagedList<GadgetResponseTest>(
                gadgetsByCategory.Items.Select(g => g.ToGadgetResponse()!).ToList(),
                gadgetsByCategory.Page,
                gadgetsByCategory.PageSize,
                gadgetsByCategory.TotalItems
            );

            return Ok(gadgetResponseList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"loi ne: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
}
