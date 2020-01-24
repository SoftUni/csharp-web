using System;

namespace SIS.HTTP
{
    public class HttpServerException : Exception
    {
        public HttpServerException(string message)
            : base(message)
        {
        }
    }
}
