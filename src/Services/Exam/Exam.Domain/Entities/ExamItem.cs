using Exam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Domain.Entities
{
    public class ExamItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationTime { get; set; }       //Exam time in seconds
        public decimal PassingScore { get; set; }   //Exam passing score
        public DateTime DateExam { get; set; }
        public ExamStatus Status { get; set; } 

        //public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }

    }
}
