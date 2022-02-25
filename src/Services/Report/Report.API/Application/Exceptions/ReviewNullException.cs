using System;

namespace Report.API.Application.Exceptions
{
    public sealed class ReviewNullException : BadRequestException
    {
        public ReviewNullException(string name)
            : base($"Entity \"{name}\" was null.")
        {

        }
    }

}
