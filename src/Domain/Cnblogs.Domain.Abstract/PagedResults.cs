using System;
using System.Collections.Generic;

namespace Cnblogs.Domain.Abstract
{
    public class PagedResults<T>
    {
        public PagedResults()
        {
            TotalCount = 0;
            Items = Array.Empty<T>();
        }

        public PagedResults(int totalCount, IReadOnlyList<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }

        public IReadOnlyList<T> Items { get; set; }

        public int TotalCount { get; set; }

        public static PagedResults<T> Empty() => new PagedResults<T>
        {
            TotalCount = 0,
            Items = Array.Empty<T>()
        };
    }
}
