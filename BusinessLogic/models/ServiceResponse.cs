using DatabaseAccess.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.models
{
    public class ServiceResponse<T>
    {
        public ServiceResponse()
        {}
        public ServiceResponse(T data, bool success, string comment)
        {
            Data = data;
            Success = success;
            Comment = comment;
        }
        public ServiceResponse(T data, bool success, string comment, ResponseType responseType)
        {
            Data = data;
            Success = success;
            Comment = comment;
            ResponseType = responseType;
        }
        public ServiceResponse(T data, bool success, string comment, long milliseconds, ResponseType responseType)
        {
            Data = data;
            Success = success;
            Comment = comment;
            Milliseconds = milliseconds;
            ResponseType = responseType;
        }

        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Comment { get; set; }
        public long Milliseconds { get; set; }
        public ResponseType ResponseType { get; set; }
    }
}
