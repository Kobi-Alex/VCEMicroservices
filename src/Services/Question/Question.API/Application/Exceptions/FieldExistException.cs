using System;

namespace Question.API.Application.Exceptions
{
    public abstract class FieldExistException : Exception
    {
        protected FieldExistException(string message)
            : base(message)
        {
        }
    }
}
