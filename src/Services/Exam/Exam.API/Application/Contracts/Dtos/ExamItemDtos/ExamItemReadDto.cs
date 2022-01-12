using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Exam.Domain.Entities;

namespace Exam.API.Application.Contracts.ExamItemDtos
{
    public class ExamItemReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationTime { get; set; }
        public decimal PassingScore { get; set; }
        public DateTime DateExam { get; set; }
        public ExamStatus Status { get; set; }
    }
}
