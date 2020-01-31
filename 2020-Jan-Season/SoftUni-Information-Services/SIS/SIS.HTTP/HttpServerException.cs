namespace SIS.HTTP
{
    using System;

    /// <summary>
    /// Represents errors that occur during HTTP Request and HTTP Response parsing.
    /// </summary>
    public class HttpServerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HttpServerException"/> class with the specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public HttpServerException(string message)
            : base(message)
        {
        }
    }
}
