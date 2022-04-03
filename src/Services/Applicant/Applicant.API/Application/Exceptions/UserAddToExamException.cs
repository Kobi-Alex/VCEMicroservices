using System;

namespace Applicant.API.Application.Exceptions
{
    public sealed class UserAddToExamException : BadRequestException
    {
        public UserAddToExamException(string mesagge)
            : base ($"Attention!! Could not add exam to user with identity {mesagge}")
        {
        }
    }
}
