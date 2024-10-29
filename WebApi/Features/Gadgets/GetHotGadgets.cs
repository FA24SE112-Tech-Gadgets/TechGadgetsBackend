using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Paginations;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Data.Entities;
using WebApi.Features.Gadgets.Models;
using FluentValidation;
using WebApi.Common.Filters;

namespace WebApi.Features.Gadgets;

[ApiController]
[RequestValidation<Request>]
public class GetHotGadgets : ControllerBase
{
    public new class Request : PagedRequest
    {
        public Guid CategoryId { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId không được để trống");
        }
    }
    [HttpGet("gadgets/hot")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Get List Of Hot Gadgets",
        Description = "API is for get list of hot gadgets. Note:" +
                            "<br>&nbsp; - Dùng API này để lấy danh sách sản phẩm có lượng người mua cao/thấp nhất." +
                            "<br>&nbsp; - Chỉ lấy ra nhưng Gadget nào Status = Active." +
                            "<br>&nbsp; - Chỉ tính Quantity những SellerOrder nào Status = Success." +
                            "<br>&nbsp; - API cũng đã sort theo thứ tự từ cao đến thấp, chỉ cần gọi ra dùng thôi (Nên gọi vài page đầu là đủ dùng rồi)."
    )]
    [ProducesResponseType(typeof(PagedList<HotGadgetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context)
    {
        bool isCategoryExist = await context.Categories.AnyAsync(c => c.Id == request.CategoryId);
        if (isCategoryExist)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("category", $"Thể loại {request.CategoryId} không tồn tại.")
            .Build();
        }
        var gadgets = await context.Gadgets
            .Include(g => g.Seller)
                .ThenInclude(s => s.User)
            .Include(g => g.FavoriteGadgets)
            .Include(g => g.SellerOrderItems)
                .ThenInclude(soi => soi.SellerOrder)
            .Where(g => g.Status == GadgetStatus.Active && g.CategoryId == request.CategoryId)
            .OrderByDescending(g => g.SellerOrderItems
                .Where(soi => soi.SellerOrder.Status == SellerOrderStatus.Success)
                .Sum(soi => soi.GadgetQuantity))
            .Select(g => new HotGadgetResponse
            {
                Id = g.Id,
                Name = g.Name,
                SellerStatus = g.Seller.User.Status,
                Quantity = g.SellerOrderItems
                    .Where(soi => soi.SellerOrder.Status == SellerOrderStatus.Success)
                    .Sum(soi => soi.GadgetQuantity)
            })
            .ToPagedListAsync(request);
        return Ok(gadgets);
    }
}
