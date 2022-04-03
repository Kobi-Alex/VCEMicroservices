using System;
using System.Runtime.Serialization;

namespace Applicant.API.Application.Exceptions
{
    public sealed class EmailAlreadyInUseException : BadRequestException
    {
        public EmailAlreadyInUseException()
            :base("Attention!! Email already in use")
        {
        }

        public EmailAlreadyInUseException(string message) :
            base($"Could not create user for {message} | Email already in use")
        {
        }
    }
}