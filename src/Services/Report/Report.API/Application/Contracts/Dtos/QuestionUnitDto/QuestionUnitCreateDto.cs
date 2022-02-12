using System;
using System.Collections.Generic;

namespace Report.API.Application.Contracts.Dtos.QuestionUnitDto
{
    public class QuestionUnitCreateDto
    {
        public int QuestionId { get; private set; }
        public string Name { get; private set; }
        public string AnswerKeys { get; private set; }
        public string CurrentKeys { get; private set; }
        public int TotalNumberAnswer { get; private set; }


        public QuestionUnitCreateDto(int questionId, string name,
            string answerKeys, string currentKeys, int totalNumberAnswer)
        {
            QuestionId = questionId;
            Name = name;
            AnswerKeys = answerKeys;
            CurrentKeys = currentKeys;
            TotalNumberAnswer = totalNumberAnswer;
        }
    }
}