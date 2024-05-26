using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int CrurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 1;
        public int TotalCount { get; set; } = 10;
        public bool HasPrevious => CrurrentPage > 0;
        public bool hasNext => CrurrentPage < TotalPages;
        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CrurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            AddRange(items);
        }
        public static PaginatedList<T> ToPageList(List<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
