using System;

namespace Applicant.API.Application.Exceptions
{
    public sealed class IncorrectPasswordException : BadRequestException
    {

        public IncorrectPasswordException(string message)
            : base(message)
        {
        }
    }
}