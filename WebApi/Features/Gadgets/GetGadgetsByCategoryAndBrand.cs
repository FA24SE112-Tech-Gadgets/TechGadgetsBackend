using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Paginations;
using WebApi.Data.Entities;
using WebApi.Data;
using WebApi.Features.Gadgets.Models;
using WebApi.Services.Auth;
using Microsoft.EntityFrameworkCore;
using WebApi.Features.Gadgets.Mappers;

namespace WebApi.Features.Gadgets;

[ApiController]
public class GetGadgetsByCategoryAndBrand : ControllerBase
{
    public new class Request : PagedRequest
    {
        public List<Guid>? Brands { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public List<Guid>? GadgetFilters { get; set; }
        public GadgetStatus? GadgetStatus { get; set; }
    }

    [HttpGet("gadgets/category/{categoryId}")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Get List Gadgets By CategoryId With Filters",
        Description = "API is for get list of gadgets by categoryId with filters. Note:" +
                            "<br>&nbsp; - Truyền field nào, filter theo field đó." +
                            "<br>&nbsp; - GadgetStatus không truyền tức là lấy cả Active và Inactive luôn" +
                            "<br>&nbsp; - GadgetFilter mỗi loại chỉ được chọn 1 thôi. Ví dụ: Ram thì chỉ 8GB ko thể vừa 8GB vừa 4GB cùng lúc." +
                            "<br>&nbsp; - Gadgets trả về là Gadget phải đáp ứng tất cả các GadgetFilter truyền vào."
    )]
    [ProducesResponseType(typeof(PagedList<GadgetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, [FromRoute] Guid categoryId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var query = context.Gadgets
            .Include(g => g.Seller)
                .ThenInclude(s => s.User)
            .Include(g => g.FavoriteGadgets)
            .Include(g => g.SpecificationValues)
            .Where(g => g.CategoryId == categoryId)
            .AsQueryable();

        if (request.GadgetStatus.HasValue)
        {
            query = query.Where(g => g.Status == request.GadgetStatus);
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(g => g.Price >= request.MinPrice.Value);
        }
        if (request.MaxPrice.HasValue)
        {
            query = query.Where(g => g.Price <= request.MaxPrice.Value);
        }

        if (request.Brands != null && request.Brands.Count > 0)
        {
            query = query.Where(g => request.Brands.Contains(g.BrandId));
        }

        var gadgets = await query
            .ToListAsync();

        int page = request.Page == null ? 1 : (int)request.Page;
        int pageSize = request.PageSize == null ? 10 : (int)request.PageSize;
        int skip = (page - 1) * pageSize;

        List<Gadget> gadgetFiltersResponse = new List<Gadget>()!;
        if (request.GadgetFilters != null && request.GadgetFilters.Count > 0)
        {
            var gadgetFilterIds = request.GadgetFilters;
            List<GadgetFilter> gadgetFilters = new List<GadgetFilter>()!;
            HashSet<Guid> specificationKeyIds = new HashSet<Guid>()!; //Dùng để check duplicate filter

            //Lấy ra danh sách các GadgetFilters muốn filter
            foreach (var gadgetFilterId in gadgetFilterIds)
            {
                var gadgetFilter = await context.GadgetFilters
                    .Include(gf => gf.SpecificationKey)
                    .FirstOrDefaultAsync(gf => gf.Id == gadgetFilterId);
                if (gadgetFilter == null)
                {
                    throw TechGadgetException.NewBuilder()
                    .WithCode(TechGadgetErrorCode.WEA_00)
                    .AddReason("gadgetFilter", $"GadgetFilterId {gadgetFilterId} không tồn tại.")
                    .Build();
                }

                if (gadgetFilter.SpecificationKey.CategoryId != categoryId)
                {
                    throw TechGadgetException.NewBuilder()
                    .WithCode(TechGadgetErrorCode.WEB_02)
                    .AddReason("gadgetFilter", $"GadgetFilterId {gadgetFilterId} không khớp với category.")
                    .Build();
                }

                // Kiểm tra trùng lặp GadgetFilter (Không cho filter vừa Ram 8GB vừa Ram 4GB cùng một lúc)
                if (!specificationKeyIds.Add(gadgetFilter.SpecificationKeyId))
                {
                    throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEA_01)
                        .AddReason("gadgetFilter", $"Không thể gadgetFilter cùng loại.")
                        .Build();
                }

                gadgetFilters.Add(gadgetFilter);
            }

            foreach (var gadget in gadgets)
            {
                // Kiểm tra xem gadget có thoả mãn tất cả các gadgetFilter không
                bool allFiltersMatch = true; // Cờ để theo dõi trạng thái
                foreach (var gadgetFilter in gadgetFilters)
                {
                    bool filterMatched = false; // Cờ cho mỗi gadgetFilter

                    foreach (var sv in gadget.SpecificationValues)
                    {
                        if (sv.SpecificationUnitId == gadgetFilter!.SpecificationUnitId
                            && sv.SpecificationKeyId == gadgetFilter.SpecificationKeyId
                            && sv.Value == gadgetFilter.Value)
                        {
                            filterMatched = true; // Thoả mãn gadgetFilter hiện tại
                            break; // Thoát khỏi vòng lặp inner
                        }
                    }

                    if (!filterMatched)
                    {
                        allFiltersMatch = false; // Nếu không có bất kỳ sv nào thỏa mãn
                        break; // Thoát khỏi vòng lặp gadgetFilter
                    }
                }

                // Nếu tất cả các gadgetFilter đã thỏa mãn, thêm gadget vào danh sách
                if (allFiltersMatch)
                {
                    gadgetFiltersResponse.Add(gadget);
                }
            }

            var gadgetResponse = gadgetFiltersResponse.ToListGadgetsResponse(currentUser != null ? currentUser.Customer!.Id : null);
            var response = new PagedList<GadgetResponse>(
                gadgetResponse!.Skip(skip).Take(pageSize).ToList(),
                page,
                pageSize,
                gadgetResponse!.Count
            );
            return Ok(response);
        }
        else
        {
            var gadgetResponse = gadgets.ToListGadgetsResponse(currentUser != null ? currentUser.Customer!.Id : null);
            var response = new PagedList<GadgetResponse>(
                gadgetResponse!.Skip(skip).Take(pageSize).ToList(),
                page,
                pageSize,
                gadgetResponse!.Count
            );
            return Ok(response);
        }
    }
}
