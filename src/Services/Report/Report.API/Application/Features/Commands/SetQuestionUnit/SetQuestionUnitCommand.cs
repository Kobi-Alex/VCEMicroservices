using System;
using MediatR;

namespace Report.API.Application.Features.Commands.SetQuestionUnit
{
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    // References on Immutable Commands:  
    // http://cqrs.nu/Faq
    // https://docs.spine3.org/motivation/immutability.html 

    public class SetQuestionUnitCommand :IRequest<bool>
    {
        public int ExamId { get; private set; }
        public string ApplicantId { get; private set; }
        public int QuestionId { get; private set; }
        public string Name { get; private set; }                //Question name
        public string AnswerKeys { get; private set; }
        public string CurrentKeys { get; private set; }
        public int TotalNumberAnswer { get; private set; }


        public SetQuestionUnitCommand(int examId, string applicantId, int questionId, string name,
            string answerKeys, string currentKeys, int totalNumberAnswer)
        {
            ExamId = examId;
            ApplicantId = applicantId;
            QuestionId = questionId;
            Name = name;
            AnswerKeys = answerKeys;
            CurrentKeys = currentKeys;
            TotalNumberAnswer = totalNumberAnswer;
        }
    }
}