using System;

namespace Applicant.API.Application.Exceptions
{
    public sealed class IncorrectAccessCodeException : BadRequestException
    {
        public IncorrectAccessCodeException()
             : base($"Incorect access code")
        {
        }
    }
}