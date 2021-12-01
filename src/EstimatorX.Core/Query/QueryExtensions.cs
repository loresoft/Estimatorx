using System.Linq.Dynamic.Core;

using AutoMapper.QueryableExtensions;

using Cosmos.Abstracts.Extensions;

using EstimatorX.Shared.Models;

using Microsoft.Azure.Cosmos.Linq;

namespace EstimatorX.Core.Query;

public static class QueryExtensions
{
    public static async Task<QueryResult<T>> ToDataResult<T>(this IQueryable<T> query, QueryRequest request, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        var dataQuery = new QueryOptions<T, T>();
        var builder = new QueryOptionsBuilder<T, T>(dataQuery);

        builder
            .Request(request)
            .Selector(t => t);

        return await ToDataResult(query, dataQuery, cancellationToken);
    }

    public static async Task<QueryResult<T>> ToDataResult<T>(this IQueryable<T> query, Action<QueryOptionsBuilder<T, T>> config, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        if (config is null)
            throw new ArgumentNullException(nameof(config));

        var dataQuery = new QueryOptions<T, T>();
        var builder = new QueryOptionsBuilder<T, T>(dataQuery);

        builder.Selector(t => t);

        config(builder);

        return await ToDataResult(query, dataQuery, cancellationToken);
    }

    public static async Task<QueryResult<TResult>> ToDataResult<TSource, TResult>(this IQueryable<TSource> query, Action<QueryOptionsBuilder<TSource, TResult>> config, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var dataQuery = new QueryOptions<TSource, TResult>();
        var builder = new QueryOptionsBuilder<TSource, TResult>(dataQuery);
        config(builder);

        return await ToDataResult(query, dataQuery, cancellationToken);
    }

    public static async Task<QueryResult<TResult>> ToDataResult<TSource, TResult>(IQueryable<TSource> query, QueryOptions<TSource, TResult> queryOptions, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (queryOptions is null)
            throw new ArgumentNullException(nameof(queryOptions));

        var filtered = query;

        // Calculate the total number of records (needed for paging)
        var total = await filtered.CountAsync(cancellationToken);

        // Sort the data    
        filtered = Sort(filtered, queryOptions.Sort, queryOptions.Descending);

        // page the data
        filtered = Page(filtered, queryOptions.Page, queryOptions.PageSize);

        // select
        var data = queryOptions.Mapper != null
            ? await filtered
                .ProjectTo<TResult>(queryOptions.Mapper)
                .ToListAsync(cancellationToken)
            : await filtered
                .Select(queryOptions.Selector)
                .ToListAsync(cancellationToken);

        return new QueryResult<TResult>
        {
            Data = data,
            Total = total
        };
    }


    public static IQueryable<T> Sort<T>(this IQueryable<T> query, string field, bool decending = false)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (string.IsNullOrEmpty(field))
            return query;

        string sort = field;
        sort += decending ? " desc" : " asc";

        return query.OrderBy(sort);
    }

    public static IQueryable<T> Page<T>(this IQueryable<T> query, int page, int pageSize)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (pageSize <= 0)
            return query;

        page = Math.Max(1, page);
        int skip = Math.Max(pageSize * (page - 1), 0);

        return query.Skip(skip).Take(pageSize);
    }

}
