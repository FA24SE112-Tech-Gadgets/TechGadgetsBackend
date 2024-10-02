using System.Linq.Expressions;

namespace WebApi.Common.Paginations;

public static class QueryableExtension
{
    public static IOrderedQueryable<T> OrderByColumn<T>(
            this IQueryable<T> query,
            Expression<Func<T, object>> sortExpression,
            SortDir? sortOrder) where T : class
    {
        return sortOrder?.ToString().ToLower() == "desc"
            ? query.OrderByDescending(sortExpression)
            : query.OrderBy(sortExpression);
    }
}
