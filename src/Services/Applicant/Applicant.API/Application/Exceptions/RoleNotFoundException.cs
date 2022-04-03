using System;

namespace Applicant.API.Application.Exceptions
{
    public sealed class RoleNotFoundException : NotFoundException
    {
        public RoleNotFoundException(string name)
            : base($"The role {name} was not found")
        {
        }

    }
}
