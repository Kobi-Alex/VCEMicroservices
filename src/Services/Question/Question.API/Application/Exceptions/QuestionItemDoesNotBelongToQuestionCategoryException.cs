using System;


namespace Question.API.Application.Exceptions
{
    public sealed class QuestionItemDoesNotBelongToQuestionCategoryException : BadRequestException
    {
        public QuestionItemDoesNotBelongToQuestionCategoryException(int questionCategoryId, int questionItemId)
            :base($"The question category with the identifier {questionCategoryId} does not belong to the question item" +
                 $"with the identifier {questionItemId}")
        {

        }
    }
}