using System;


namespace Report.API.Application.Exceptions
{
    public sealed class ReviewNotFoundException : NotFoundException
    {
        public ReviewNotFoundException(string name, object key)
           : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}