using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gold_server.DTOs.common
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponseDto() { }

        public ApiResponseDto(T data, string message = null)
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public ApiResponseDto(string message)
        {
            Success = false;
            Message = message;
        }
    }
}