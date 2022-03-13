using System;


namespace Report.API.Application.Exceptions
{
    public sealed class ReviewIsExistException : BadRequestException
    {
        public ReviewIsExistException(string name)
            : base($"The applicant cannot the same exam #{name}. Please notify to admin or teacher!")
        {
        }
    }

}
