using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Domain.Contracts.Dtos
{
    public class ExamReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int DurationTime { get; set; }
        public decimal PassingScore { get; set; }
        public DateTime DateExam { get; set; }
        public string Status { get; set; }
    }
}
