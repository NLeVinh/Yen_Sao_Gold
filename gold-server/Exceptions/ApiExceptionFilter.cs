using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gold_server.DTOs.common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace gold_server.Exceptions
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception");

            var message = context.Exception switch
            {
                AppException appEx => appEx.Message,
                _ => "Internal server error"
            };

            var statusCode = context.Exception is AppException ? 400 : 500;

            var apiResponse = new ApiResponseDto<string>(message)
            {
                Success = false
            };

            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}