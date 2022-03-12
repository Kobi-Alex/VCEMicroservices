
using System;


namespace Question.API.Application.Exceptions
{
    public sealed class QuestionAnswerFieldException: BadRequestException
    {
        public QuestionAnswerFieldException(string key)
           : base($"Attention!! The answer with the same char key --> {key} is exist")
        {
        }
    }
}
