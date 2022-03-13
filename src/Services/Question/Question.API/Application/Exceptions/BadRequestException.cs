using System;


namespace Question.API.Application.Exceptions
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message)
            : base(message)
        {
        }
    }
}
