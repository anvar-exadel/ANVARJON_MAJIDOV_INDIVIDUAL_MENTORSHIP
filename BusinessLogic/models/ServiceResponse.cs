using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.models
{
    public class ServiceResponse<T>
    {
        public ServiceResponse()
        {}
        public ServiceResponse(T data, bool success, string comment) {
            Data = data;
            Success = success;
            Comment = comment;
        }

        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Comment { get; set; }
    }
}
