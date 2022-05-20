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
        public ServiceResponse(T data)
        {
            Data = data;
        }
        public ServiceResponse(T data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }
        public ServiceResponse(T data, bool success, long milliseconds, ResponseType responseType)
        {
            Data = data;
            Success = success;
            Milliseconds = milliseconds;
            ResponseType = responseType;
        }
        public ServiceResponse(T data, bool success, ResponseType responseType)
        {
            Data = data;
            Success = success;
            ResponseType = responseType;
        }
        public ServiceResponse(T data, bool success, string message, ResponseType responseType)
        {
            Data = data;
            Success = success;
            Message = message;
            ResponseType = responseType;
        }
        public ServiceResponse(T data, bool success, string message, long milliseconds, ResponseType responseType)
        {
            Data = data;
            Success = success;
            Message = message;
            Milliseconds = milliseconds;
            ResponseType = responseType;
        }

        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public long Milliseconds { get; set; }
        public ResponseType ResponseType { get; set; }
    }
}
