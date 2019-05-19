namespace SIS.HTTP.Exceptions
{
    using System;
    public class InternalServerErrorException : Exception
    {
        private const string InternalServerErrorExceptionDefaultMessage = "The Server has encountered an error.";

        public InternalServerErrorException() 
            : this(InternalServerErrorExceptionDefaultMessage)
        {

        }

        public InternalServerErrorException(string name) 
            : base(name)
        {

        }
    }
}
