using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccess.models
{
    public class DbResponse<T>
    {
        public DbResponse(T data, bool success, string message, ResponseType responseType)
        {
            Data = data;
            Success = success;
            Message = message;
            ResponseType = responseType;
        }
        public DbResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; } = ResponseType.Success;
    }

    public enum ResponseType
    {
        Success, Failed, Canceled
    }
}
