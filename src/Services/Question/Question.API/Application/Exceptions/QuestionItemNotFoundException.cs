using System;


namespace Question.API.Application.Exceptions
{
    public sealed class QuestionItemNotFoundException: NotFoundException
    {
        public QuestionItemNotFoundException(int Id)
           : base($"The question with the identifier {Id} was not found")
        {
        }
    }
}
