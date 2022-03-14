using System;


namespace Report.API.Application.Exceptions
{
    public sealed class QuestionUnitCurrentKeyException : BadRequestException
    {
        public QuestionUnitCurrentKeyException(int id)
           : base($"Current key with Question id: \"{id}\" must have been next schema A,B,C..")
        {
        }
    }
}