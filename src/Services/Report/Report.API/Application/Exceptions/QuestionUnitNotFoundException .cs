using System;


namespace Report.API.Application.Exceptions
{
    public sealed class QuestionUnitNotFoundException : NotFoundException
    {
        public QuestionUnitNotFoundException(int id)
           : base($"Entity with id: \"{id}\" was not found.")
        {
        }
    }
}