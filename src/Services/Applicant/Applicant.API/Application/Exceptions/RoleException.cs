using System;

namespace Applicant.API.Application.Exceptions
{
    public sealed class RoleException : BadRequestException
    {

        public RoleException()
            : base($"Attention!! The role has the same name already")
        {
        }
        public RoleException(string message)
            : base($"Cannot add role: < {message} > to user with roles: Admin, Manager or Teacher")
        {
        } 

    }
}
