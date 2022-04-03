using System;

namespace Applicant.API.Application.Exceptions
{
    public sealed class AccessCodeIncorrectException : BadRequestException
    {
        public AccessCodeIncorrectException()
            : base("Attention!! Incorect access code")
        {
        }

    }
}
