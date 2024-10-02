using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Common.Paginations;

public class PagedRequest
{
    public const int MaxPageSize = 100;
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

public class PagedRequestValidator<T> : AbstractValidator<T> where T : PagedRequest
{
    public PagedRequestValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(PagedRequest.MaxPageSize);
    }
}

public record PagedList<T>(List<T> Items, int Page, int PageSize, int TotalItems)
{
    public bool HasNextPage => Page * PageSize < TotalItems;
    public bool HasPreviousPage => Page > 1;
}

public static class PaginationDatabaseExtensions
{
    public static async Task<PagedList<TResponse>> ToPagedListAsync<TRequest, TResponse>(this IQueryable<TResponse> query, TRequest request) where TRequest : PagedRequest
    {
        var page = request.Page ?? 1;
        var pageSize = request.PageSize ?? 10;

        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(page, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pageSize, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(pageSize, PagedRequest.MaxPageSize);

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<TResponse>(items, page, pageSize, totalItems);
    }
}
