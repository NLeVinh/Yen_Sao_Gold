using System;

namespace gold_server.Exceptions
{
    public class AppException : System.Exception
    {
        public AppException(string message) : base(message) { }
        public AppException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}