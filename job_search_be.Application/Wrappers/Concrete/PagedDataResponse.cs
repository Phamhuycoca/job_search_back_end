using job_search_be.Application.Wrappers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Wrappers.Concrete
{
    public class PagedDataResponse<T> : IPagedDataResponse<T>
    {
        public bool Success { get; } = true;
        public int TotalItems { get; }

        public List<T> Items { get; }

        public int StatusCode { get; }

        public PagedDataResponse(List<T> data, int statuscode, int totalitems)
        {
            Items = data;
            StatusCode = statuscode;
            TotalItems = totalitems;
        }
    }
}
