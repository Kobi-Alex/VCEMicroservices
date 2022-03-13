using System;


namespace Exam.API.Application.Exceptions
{
    public abstract class BadRequestException : Exception 
    {
        protected BadRequestException(string message)
            : base(message)
        {
        }
    }
}
