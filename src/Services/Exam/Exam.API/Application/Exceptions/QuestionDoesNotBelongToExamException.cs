using System;

namespace Exam.API.Application.Exceptions
{
    public sealed class QuestionDoesNotBelongToExamException : BadRequestException
    {
        public QuestionDoesNotBelongToExamException(int examId, int questionId)
             : base($"The question with the identifier {questionId} does not belong to the exam" +
                   $" with the identifier {examId}")
        {
        }
    }
        
}
