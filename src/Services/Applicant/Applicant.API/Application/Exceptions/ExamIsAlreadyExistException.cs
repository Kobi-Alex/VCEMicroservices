using System;


namespace Applicant.API.Application.Exceptions
{
    public sealed class ExamIsAlreadyExistException : BadRequestException
    {
        public ExamIsAlreadyExistException(int id)
            :base ($"Exam with identity {id} is already exist")
        {
        }
    }
}