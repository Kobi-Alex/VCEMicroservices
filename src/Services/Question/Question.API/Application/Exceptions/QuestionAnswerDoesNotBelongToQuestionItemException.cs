using System;

namespace Question.API.Application.Exceptions
{
    public sealed class QuestionAnswerDoesNotBelongToQuestionItemException : BadRequestException
    {
        public QuestionAnswerDoesNotBelongToQuestionItemException(int questionItemId)
            : base($"The question answer does not belong to the question item" +
                 $"with the identifier {questionItemId}")
        {
        }
    }
}
