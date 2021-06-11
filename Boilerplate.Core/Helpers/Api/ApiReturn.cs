using System;

namespace Boilerplate.Core.Helpers.Api
{
    public class ApiReturn
    {
        public string ApiVersion { get; set; } = "1.0";
        public int StatusCode { get; set; }
        public double ResponseTime { get; set; }
        public DateTime UtcTimestamp { get; set; } = DateTime.UtcNow;
        public object Data { get; set; }
        public bool IsErrorOccured { get; set; }
        public Error Error { get; set; }

        public bool ShouldSerializeError()
        {
            return IsErrorOccured;
        }

        public bool ShouldSerializeData()
        {
            return !IsErrorOccured;
        }

        public static ApiReturn ErrorResponse(Error error, int statusCode)
        {
            return new()
            {
                Error = error,
                IsErrorOccured = true,
                StatusCode = statusCode
            };
        }
    }
}