using System;

namespace Applicant.API.Application.Exceptions
{
    public sealed class RoleDoesNotBelongToUserException : BadRequestException
    {
        public RoleDoesNotBelongToUserException(string userId, int roleId)
             : base($"The role with the identifier {roleId} does not belong to the user" +
                   $" with the identifier {userId}")
        {
        }
    }
}