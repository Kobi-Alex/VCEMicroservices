using System;


namespace Question.API.Application.Exceptions
{
    public sealed class QuestionCategoryArgumentException : BadRequestException
    {
        public QuestionCategoryArgumentException(string message)
            : base($"The entity {message} is null")
        {
        }
    }
}
