using System;


namespace Applicant.API.Application.Exceptions
{
    public sealed class ExamIsAlreadyExistException : BadRequestException
    {
        public ExamIsAlreadyExistException(int id)
            :base ($"Exam with identity {id} is already exist")
        {
        }

        public ExamIsAlreadyExistException(string message)
            : base($"Exam is already exist in {message}")
        {
        }
    }
}