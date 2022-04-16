using System;

namespace Question.API.Application.Exceptions
{
    public sealed class QuestionItemDeleteException : BadRequestException
    {
        public QuestionItemDeleteException(string message)
            :base(message)
        {
        }
    }
}