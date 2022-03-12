using System;


namespace Question.API.Application.Exceptions
{
    public sealed class QuestionCategoryNotFoundException : NotFoundException
    {
        public QuestionCategoryNotFoundException(int Id)
            : base($"The question category with the identifier {Id} was not found")
        {
        }
    }
}
