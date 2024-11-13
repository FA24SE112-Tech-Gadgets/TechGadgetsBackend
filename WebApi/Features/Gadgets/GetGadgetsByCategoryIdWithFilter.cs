using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Gadgets.Mappers;
using WebApi.Features.Gadgets.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Gadgets;

[ApiController]
public class GetGadgetsByCategoryIdWithFilter : ControllerBase
{
    public new class Request : PagedRequest
    {
        public List<Guid>? Brands { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public List<Guid>? GadgetFilters { get; set; }
        public GadgetStatus? GadgetStatus { get; set; }
    }

    public class RequestValidator : PagedRequestValidator<Request>;

    [HttpGet("gadgets/category/{categoryId}")]
    [Tags("Gadgets")]
    [SwaggerOperation(
        Summary = "Get List Gadgets By CategoryId With Filters",
        Description = "API is for get list of gadgets by categoryId with filters. Note:" +
                            "<br>&nbsp; - Truyền field nào, filter theo field đó." +
                            "<br>&nbsp; - GadgetStatus không truyền tức là lấy cả Active và Inactive luôn" +
                            "<br>&nbsp; - GadgetFilter mỗi loại được chọn nhiều. Ví dụ: Ram có thể vừa chọn 8GB vừa 4GB cùng lúc." +
                            "<br>&nbsp; - Gadgets trả về là Gadget phải đáp ứng tất cả các GadgetFilter truyền vào. Ví dụ vừa phải có Ram 8GB hoặc 4GB vừa phải có Hệ điều hành: Android"
    )]
    [ProducesResponseType(typeof(PagedList<GadgetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, [FromRoute] Guid categoryId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (!await context.Categories.AnyAsync(b => b.Id == categoryId))
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("category", "Thể loại không tồn tại")
                        .Build();
        }

        var query = context.Gadgets
            .Include(g => g.Seller)
                .ThenInclude(s => s.User)
            .Include(g => g.FavoriteGadgets)
            .Include(g => g.GadgetDiscounts)
            .Include(g => g.SpecificationValues)
            .Where(g => g.CategoryId == categoryId && g.Status == GadgetStatus.Active && g.Seller.User.Status == UserStatus.Active)
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

                gadgetFilters.Add(gadgetFilter);
            }

            // Group gadgetFilters by SpecificationKeyId
            var groupedGadgetFilters = gadgetFilters
                .GroupBy(gf => gf.SpecificationKeyId)
                .ToList();

            List<Gadget> test = new List<Gadget>()!;
            // Filter gadgets based on grouped gadgetFilters
            foreach (var gadget in gadgets)
            {
                // Ensure the gadget matches at least one filter from each filter group
                bool allGroupsMatch = groupedGadgetFilters.All(gadgetFilterGroup =>
                {
                    return gadgetFilterGroup.Any(gadgetFilter =>
                    {
                        return gadget.SpecificationValues.Any(sv =>
                            sv.SpecificationUnitId == gadgetFilter.SpecificationUnitId
                            && sv.SpecificationKeyId == gadgetFilter.SpecificationKeyId
                            && (
                                (gadgetFilter.IsFilteredByVector && CalculateL2Distance(sv.Vector.ToArray(), gadgetFilter.Vector.ToArray()) < 1)
                                || (!gadgetFilter.IsFilteredByVector && sv.Value == gadgetFilter.Value)
                                )
                            ) && gadgetFilter.Value != "Khác";
                    });
                });

                // If the gadget meets all group conditions, add to the response list
                if (allGroupsMatch)
                {
                    gadgetFiltersResponse.Add(gadget);
                }
            }

            var gadgetResponse = gadgetFiltersResponse.ToListGadgetsResponse(currentUser != null ? currentUser?.Customer?.Id : null);
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
            var gadgetResponse = gadgets.ToListGadgetsResponse(currentUser != null ? currentUser?.Customer?.Id : null);
            var response = new PagedList<GadgetResponse>(
                gadgetResponse!.Skip(skip).Take(pageSize).ToList(),
                page,
                pageSize,
                gadgetResponse!.Count
            );
            return Ok(response);
        }
    }
    private static float CalculateL2Distance(float[] v1, float[] v2)
    {
        if (v1.Length != v2.Length)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("vector", "Vector phải có cùng độ dài.")
            .Build();
        }


        float sum = 0.0f;

        for (int i = 0; i < v1.Length; i++)
        {
            float difference = v1[i] - v2[i];
            sum += difference * difference;
        }

        return (float)Math.Sqrt(sum);
    }
}

