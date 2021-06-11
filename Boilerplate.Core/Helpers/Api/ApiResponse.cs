using System.Collections.Generic;

namespace Boilerplate.Core.Helpers.Api
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string InternalMessage { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ApiResponse : ApiResponse<object>
    {
        public string ApiVersion { get; internal set; }
    }
}