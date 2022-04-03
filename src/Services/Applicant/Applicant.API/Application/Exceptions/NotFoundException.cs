using System;

namespace Applicant.API.Application.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message)
            : base(message)
        {
        }
    }
}
