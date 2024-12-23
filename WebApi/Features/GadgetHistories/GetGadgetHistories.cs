﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.GadgetHistories.Mappers;
using WebApi.Features.GadgetHistories.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.GadgetHistories;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class GetGadgetHistories : ControllerBase
{
    public new class Request : PagedRequest
    {
        public SortByDate SortByDate { get; set; }
    }

    public enum SortByDate
    {
        DESC, ASC
    }

    [HttpGet("gadget-histories")]
    [Tags("Gadget Histories")]
    [SwaggerOperation(
        Summary = "Get List Of Customer Gadget Histories",
        Description = "API is for get list of customer gadget histories. Note:" +
                            "<br>&nbsp; - SortByDate: 'DESC' - Ngày gần nhất, 'ASC' - Ngày xa nhất. Nếu không truyền defaul: 'DESC'"
    )]
    [ProducesResponseType(typeof(PagedList<GadgetHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        //var currentUser = await currentUserService.GetCurrentUser();

        //string sortBy = request.SortByDate == SortByDate.ASC ? "ASC" : "DESC";
        //var query = $@"
        //    WITH RankedHistories AS (
        //        SELECT 
        //            gh.*,
        //            ROW_NUMBER() OVER (PARTITION BY gh.""GadgetId"" ORDER BY gh.""CreatedAt"" DESC) AS rank
        //        FROM ""GadgetHistories"" gh
        //        WHERE gh.""CustomerId"" = '{currentUser!.Customer!.Id}'
        //    )
        //    SELECT 
        //        rh.""Id"", rh. ""GadgetId"", rh.""CustomerId"", rh.""CreatedAt""
        //    FROM RankedHistories rh
        //    WHERE rh.rank = 1
        //    ORDER BY rh.""CreatedAt"" {sortBy}
        //    LIMIT {request.PageSize ?? 10} OFFSET {request.PageSize ?? 10} * ({request.Page ?? 1} - 1)
        //";
        //var totalUniqueGadgets = context.GadgetHistories
        //    .Where(gh => gh.CustomerId == currentUser!.Customer!.Id)
        //    .Select(gh => gh.GadgetId)
        //    .Distinct()
        //    .Count();

        //var gadgetHistories = await context.GadgetHistories
        //    .FromSqlRaw(query)
        //    .Include(gh => gh.Gadget)
        //        .ThenInclude(g => g.Brand)
        //    .Include(gh => gh.Gadget)
        //        .ThenInclude(g => g.Seller)
        //        .ThenInclude(s => s.User)
        //    .Include(gh => gh.Gadget)
        //        .ThenInclude(g => g.Category)
        //    .Include(gh => gh.Gadget)
        //        .ThenInclude(g => g.FavoriteGadgets)
        //    .Include(gh => gh.Gadget)
        //        .ThenInclude(g => g.GadgetDiscounts)
        //    .ToListAsync();
        //var gadgetHistoryResponseList = new PagedList<GadgetHistoryResponse>(
        //    gadgetHistories.Select(gh => gh.ToGadgetHistoryResponse(currentUser.Customer.Id)!).ToList(),
        //    request.Page ?? 1,
        //    request.PageSize ?? 10,
        //    totalUniqueGadgets // Đếm tổng số bản ghi
        //);
        //return Ok(gadgetHistoryResponseList);
        var user = await currentUserService.GetCurrentUser();

        var query = context.GadgetHistories
                            .Include(gh => gh.Gadget.Brand)
                            .Include(gh => gh.Gadget.Seller)
                            .Include(gh => gh.Gadget.Category)
                            .Include(gh => gh.Gadget.FavoriteGadgets)
                            .Include(gh => gh.Gadget.GadgetDiscounts)
                            .Where(gh => gh.CustomerId == user!.Customer!.Id)
                            .AsQueryable();

        if (request.SortByDate == SortByDate.DESC)
        {
            query = query.OrderByDescending(gh => gh.CreatedAt);
        }
        else
        {
            query = query.OrderBy(gh => gh.CreatedAt);
        }

        var response = await query
                            .Select(gh => gh.ToGadgetHistoryResponse(user!.Customer!.Id))
                            .ToPagedListAsync(request);

        return Ok(response);
    }
}
