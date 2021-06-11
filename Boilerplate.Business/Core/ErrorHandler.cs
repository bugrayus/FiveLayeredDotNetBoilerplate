using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Boilerplate.Core.Helpers.Api;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Boilerplate.Business.Core
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;

        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException exception)
            {
                var code = exception switch
                {
                    { } => HttpStatusCode.InternalServerError
                };
                await HandleExceptionAsync(context, exception, code);
            }
            catch (Exception ex)
            {
                var error = new Error
                {
                    Message = $"Undefined error occured. Message: {ex.Message}",
                    StackTrace = ex.StackTrace
                };
                var code = ex switch
                {
                    ApiException => HttpStatusCode.InternalServerError,
                    KeyNotFoundException => HttpStatusCode.NotFound,
                    _ => HttpStatusCode.BadRequest
                };
                await HandleExceptionAsync(context, new ApiException(error), code);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, ApiException exception, HttpStatusCode code)
        {
            var resp = ApiReturn.ErrorResponse(exception.Error, (int) code);
            var result = JsonConvert.SerializeObject(resp);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(result);
        }
    }
}