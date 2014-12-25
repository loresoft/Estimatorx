using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Estimatorx.Core.Query
{
    public static class QueryExtensions
    {
        public static QueryResult<T> ToDataResult<T>(this IQueryable<T> query, QueryRequest request)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            var dataQuery = new QueryOptions<T, T>();
            var builder = new QueryOptionsBuilder<T, T>(dataQuery);
            builder
                .Request(request)
                .Selector(t => t);

            return ToDataResult(query, dataQuery);
        }

        public static QueryResult<T> ToDataResult<T>(this IQueryable<T> query, Action<QueryOptionsBuilder<T, T>> config)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            var dataQuery = new QueryOptions<T, T>();
            var builder = new QueryOptionsBuilder<T, T>(dataQuery);
            builder.Selector(t => t);

            config(builder);

            return ToDataResult(query, dataQuery);
        }
        
        public static QueryResult<TResult> ToDataResult<TSource, TResult>(this IQueryable<TSource> query, Action<QueryOptionsBuilder<TSource, TResult>> config)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            var dataQuery = new QueryOptions<TSource, TResult>();
            var builder = new QueryOptionsBuilder<TSource, TResult>(dataQuery);
            config(builder);

            return ToDataResult(query, dataQuery);
        }


        private static QueryResult<TResult> ToDataResult<TSource, TResult>(IQueryable<TSource> query, QueryOptions<TSource, TResult> queryOptions )
        {
            // Calculate the total number of records (needed for paging)
            var total = query.Count();

            // Sort the data    
            query = Sort(query, queryOptions.Sort, queryOptions.Descending);

            // page the data
            query = Page(query, queryOptions.Page, queryOptions.PageSize);

            // select
            var data = query.Select(queryOptions.Selector).ToList();
            
            // get page count
            int pageCount = total > 0 ? (int)Math.Ceiling((double)total / queryOptions.PageSize) : 0;

            return new QueryResult<TResult>
            {
                Data = data,

                Page = queryOptions.Page,
                PageSize = queryOptions.PageSize,
                PageCount = pageCount,

                Sort = queryOptions.Sort,
                Descending = queryOptions.Descending,

                Total = total
            };
        }


        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string field, bool decending = false)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            if (string.IsNullOrEmpty(field))
                return query;

            string sort = field;
            sort += decending ? " desc" : " asc";

            return query.OrderBy(sort);
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            if (pageSize <= 0)
                return query;

            page = Math.Max(1, page);
            int skip = Math.Max(pageSize * (page - 1), 0);

            return query.Skip(skip).Take(pageSize);
        }
    }
}