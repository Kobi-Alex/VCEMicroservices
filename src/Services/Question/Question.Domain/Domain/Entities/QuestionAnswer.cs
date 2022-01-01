using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Domain.Entities
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public decimal CorrectAnswerCoefficient { get; set; }

        public int QuestionItemId { get; set; }
        public QuestionItem QuestionItem { get; set; }
    }
}