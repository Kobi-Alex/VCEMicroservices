using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Applicant.API.Application.Exceptions
{
    public class ExamNotFoundInUserException: BadRequestException
    {
        public ExamNotFoundInUserException(int id) : base($"Exam  {id} not found in user")
        {
        }
    }
}
