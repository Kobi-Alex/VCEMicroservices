using System;

namespace Exam.API.Application.Contracts.ExamQuestionDtos
{
    public class ExamQuestionReadDto
    {
        public int Id { get; set; }
        public int QuestionItemId { get; set; }
        public int ExamItemId { get; set; }
    }
}
