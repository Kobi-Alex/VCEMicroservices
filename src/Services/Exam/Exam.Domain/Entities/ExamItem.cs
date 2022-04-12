using System;
using System.Collections.Generic;


namespace Exam.Domain.Entities
{
    public class ExamItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationTime { get; set; }       //Exam time in seconds
        public decimal PassingScore { get; set; }   //Exam passing score in (%)

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }

    }
}
