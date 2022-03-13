using System;
using System.Collections.Generic;


namespace Report.API.Application.Features.Queries
{

    //CQRS pattern (Queries) comment:
    //The {init} accessor makes immutable objects more flexible by allowing the caller to mutate the members
    //during the act of construction. That means the object's immutable properties can participate in object
    //initializers and thus removes the need for all constructor boilerplate in the type. 

    public record Review
    {
        public int Id { get; init; }                      // ID
        public int ExamId { get; init; }                  // Exam id
        public string ApplicantId { get; init; }          // Applicant id (userId)
        public DateTime ReportDate { get; init; }         // Report date
        public decimal TotalScore { get; init; }          // Count of correct answers (number)
        public decimal PersentScore { get; init; }        // Count of correct answers in percent (%)
        public string Grade { get; init; }                // Grade (A,B,C,D,F..)

        public List<QuestionUnit> QuestionUnits { get; set; } = new List<QuestionUnit>();  // List of correct answers on the current question

    }

    public record QuestionUnit
    {
        public string QuestionName { get; init; }        // Question
        public string AnswerKeys { get; init; }          // Correct answer in the form of a tape [C, D, E]
        public string CurrentKeys { get; init; }         // Current applicant answer in the form of a tape [A, B, C, D, E]
        public int TotalNumberAnswer { get; init; }      // Total number of answers to the question
        public int QuestionId { get; init; }             // Current question Id
    }
}
