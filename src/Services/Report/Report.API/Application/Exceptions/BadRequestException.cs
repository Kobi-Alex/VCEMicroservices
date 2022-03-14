using System;


namespace Report.API.Application.Exceptions
{
    public abstract class BadRequestException : Exception 
    {
        protected BadRequestException(string message)
            : base(message)
        {
        }
    }
}
