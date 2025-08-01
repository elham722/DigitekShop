using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace DigitekShop.Application.Responses
{
    public static class ResponseWrapper
    {
        public static T Wrap<T>(T response, HttpContext? httpContext = null) where T : BaseResponse
        {
            if (httpContext != null)
            {
                // Add request ID if available
                if (httpContext.TraceIdentifier != null)
                {
                    response.RequestId = httpContext.TraceIdentifier;
                }

                // Add correlation ID if available in headers
                if (httpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
                {
                    response.RequestId = correlationId.ToString();
                }
            }

            return response;
        }

        public static SuccessResponse<T> WrapSuccess<T>(T data, string message = "Operation completed successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateSuccess(data, message);
            return Wrap(response, httpContext);
        }

        public static SuccessResponse<T> WrapPagedSuccess<T>(T data, int totalCount, int pageNumber, int pageSize, string message = "Operation completed successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreatePagedSuccess(data, totalCount, pageNumber, pageSize, message);
            return Wrap(response, httpContext);
        }

        public static CommandResponse<T> WrapCommand<T>(string operation, string message = "Command executed successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateCommand<T>(operation, message);
            return Wrap(response, httpContext);
        }

        public static CommandResponse<T> WrapCommandWithData<T>(T data, string operation, string message = "Command executed successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateCommandWithData(data, operation, message);
            return Wrap(response, httpContext);
        }

        public static CommandResponse<T> WrapCommandWithId<T>(int id, string operation, string message = "Command executed successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateCommandWithId<T>(id, operation, message);
            return Wrap(response, httpContext);
        }

        public static CommandResponse<T> WrapCommandWithDataAndId<T>(T data, int id, string operation, string message = "Command executed successfully", HttpContext? httpContext = null)
        {
            var response = ResponseFactory.CreateCommandWithDataAndId(data, id, operation, message);
            return Wrap(response, httpContext);
        }
    }
} 