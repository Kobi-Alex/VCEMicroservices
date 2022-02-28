using System;

namespace Question.API.Application.Exceptions
{
    public sealed class QuestionItemArgumentException : BadRequestException
    {
        public QuestionItemArgumentException(string message)
            : base($"The entity {message} is null")
        {
        }
    }
}
