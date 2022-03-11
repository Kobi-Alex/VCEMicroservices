using System;

namespace Report.API.Application.Exceptions
{
    public sealed class ExamItemNotFoundException : NotFoundException
    {
        public ExamItemNotFoundException(int id)
           : base($"Exam item with id: \"{id}\" was not found.")
        {
        }
    }
}