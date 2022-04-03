using System;

namespace Applicant.API.Application.Exceptions
{
    public sealed class InvalidLoginException : BadRequestException
    {
        public InvalidLoginException()
            : base($"Invalid login request")
        {
        }
    }
}