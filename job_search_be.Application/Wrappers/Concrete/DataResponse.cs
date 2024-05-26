using job_search_be.Application.Wrappers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace job_search_be.Application.Wrappers.Concrete
{
    public class DataResponse<T> : IDataResponse<T>
    {
        public bool Success { get; } = true;
        public T Data { get; }

        public int StatusCode { get; }
        public string Message { get; set; }

        [JsonConstructor]
        public DataResponse(T data, int statuscode)
        {
            Data = data;
            StatusCode = statuscode;
        }

        public DataResponse(T data, int statuscode, string message)
        {
            Data = data;
            StatusCode = statuscode;
            Message = message;
        }
    }
}
