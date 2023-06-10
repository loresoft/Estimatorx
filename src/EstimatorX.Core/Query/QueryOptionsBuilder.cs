using System.Linq.Expressions;

using AutoMapper;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Query;

public class QueryOptionsBuilder<TSource, TResult>
{
    public QueryOptions<TSource, TResult> QueryOptions { get; }

    public QueryOptionsBuilder(QueryOptions<TSource, TResult> queryOptions)
    {
        QueryOptions = queryOptions;
    }

    public QueryOptionsBuilder<TSource, TResult> Request(QueryRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        QueryOptions.Page = request.Page ?? 1;
        QueryOptions.PageSize = request.PageSize ?? 20;
        QueryOptions.Sort = request.Sort;
        QueryOptions.Descending = request.Descending ?? false;

        return this;
    }

    public QueryOptionsBuilder<TSource, TResult> Page(int page)
    {
        QueryOptions.Page = page;
        return this;
    }

    public QueryOptionsBuilder<TSource, TResult> PageSize(int pageSize)
    {
        QueryOptions.PageSize = pageSize;
        return this;
    }

    public QueryOptionsBuilder<TSource, TResult> Sort(string sortField)
    {
        QueryOptions.Sort = sortField;
        return this;
    }

    public QueryOptionsBuilder<TSource, TResult> Descending(bool value = true)
    {
        QueryOptions.Descending = value;
        return this;
    }

    public QueryOptionsBuilder<TSource, TResult> Selector(Expression<Func<TSource, TResult>> selector)
    {
        if (selector == null)
            throw new ArgumentNullException(nameof(selector));

        QueryOptions.Selector = selector;
        return this;
    }

    public QueryOptionsBuilder<TSource, TResult> Mapper(IConfigurationProvider configurationProvider)
    {
        if (configurationProvider is null)
            throw new ArgumentNullException(nameof(configurationProvider));

        QueryOptions.Mapper = configurationProvider;
        return this;
    }
}
