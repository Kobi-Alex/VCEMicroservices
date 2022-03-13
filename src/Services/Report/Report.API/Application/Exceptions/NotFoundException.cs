using System;


namespace Report.API.Application.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message)
            : base(message)
        {
        }
    }
}