using System;

namespace Question.API.Application.Exceptions
{
    public class QuestionAnswerQuantityLimitException : QuantityLimitException
    {
        public QuestionAnswerQuantityLimitException() 
            : base($"Attention! The max count of the answer in one question do not must exceed 5 items")
        {
        }
    }
}