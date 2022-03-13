using System;


namespace Question.API.Application.Exceptions
{
    public sealed class QuestionAnswerNotFoundException : NotFoundException
    {
        public QuestionAnswerNotFoundException(int questionId)
            : base($"The question answer with the identifier {questionId} was not found")
        {
        }
    }
}