using System;


namespace Question.API.Application.Exceptions
{
    public sealed class QuestionAnswerArgumentException : BadRequestException
    {
        public QuestionAnswerArgumentException(string message)
            : base($"The entity {message}")
        {
        }
    }
}
