using System;


namespace Exam.Domain.Entities
{
    public class ExamQuestion
    {
        public int Id { get; set; }
        public int QuestionItemId { get; set; }

        public int ExamItemId { get; set; }
        public ExamItem ExamItem { get; set; }
    }
}
