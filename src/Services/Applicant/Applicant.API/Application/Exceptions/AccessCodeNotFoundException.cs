using System;

namespace Applicant.API.Application.Exceptions
{
    public sealed class AccessCodeNotFoundException : NotFoundException
    {
        public AccessCodeNotFoundException()
            : base("The access codes were not found")
        {
        }

    }
}
