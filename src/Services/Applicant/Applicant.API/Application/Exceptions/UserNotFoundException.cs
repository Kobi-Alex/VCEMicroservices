using System;

namespace Applicant.API.Application.Exceptions
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string Id)
            : base($"The user with the identifier {Id} was not found")
        {
        }

        public UserNotFoundException(string Id, string message)
            :base($"Error delete user id = {Id}. {message}")
        {
        }

    }
}
