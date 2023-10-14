using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreClient.Models
{
    public class PageList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PageList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public static PageList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items =  source.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToList();
            return new PageList<T>(items, count, pageIndex, pageSize);
        }
    }
}
