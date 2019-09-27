using System;
using System.Collections.Generic;

namespace Cnblogs.Domain.Abstract
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
            TotalCount = 0;
            Items = Array.Empty<T>();
        }

        public PagedResult(int totalCount, IReadOnlyList<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }

        public IReadOnlyList<T> Items { get; set; }

        public int TotalCount { get; set; }

        public static PagedResult<T> Empty() => new PagedResult<T>
        {
            TotalCount = 0,
            Items = Array.Empty<T>()
        };
    }
}
