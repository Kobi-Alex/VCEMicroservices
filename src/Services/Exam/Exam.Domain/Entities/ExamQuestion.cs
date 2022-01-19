using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Domain.Entities
{
    public class ExamQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int QuestionItemId { get; set; }

        public int ExamItemId { get; set; }
        public ExamItem ExamItem { get; set; }
    }
}
