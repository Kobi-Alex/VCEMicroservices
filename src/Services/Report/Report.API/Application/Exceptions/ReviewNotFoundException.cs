using System;


namespace Report.API.Application.Exceptions
{
    public sealed class ReviewNotFoundException : NotFoundException
    {

        public ReviewNotFoundException(string message)
            : base($"Review number {message} was not found.")
        {
        }

        public ReviewNotFoundException(string name, object key)
           : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}