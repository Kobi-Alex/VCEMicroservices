using System;

namespace Question.API.Application.Exceptions
{
    public abstract class QuantityLimitException : Exception
    {
        protected QuantityLimitException()
        {
        }

        protected QuantityLimitException(string message)
            :base(message)
        {
        }
    }
}