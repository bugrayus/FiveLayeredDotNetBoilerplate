using System;

namespace Boilerplate.Core.Helpers.Api
{
    public class ApiException : Exception
    {
        public Error Error { get; set; }
        public ApiException(Error error)
        {
            Error = error;
        }
    }

    public class Error
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public bool ShouldSerializeStackTrace()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
