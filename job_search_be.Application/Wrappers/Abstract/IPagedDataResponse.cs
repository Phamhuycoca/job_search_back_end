using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Wrappers.Abstract
{
    public interface IPagedDataResponse<T> : IResponse
    {
        int TotalItems { get; }
        List<T> Items { get; }
    }
}
