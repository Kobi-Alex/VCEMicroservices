using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Contracts.Dtos
{
    public class QuestionReadDto
    {
        public int Id { get; set; }
        public string Category { get; set; }

        //Question
        public string Question { get; set; }
        public string QuestionOptionA { get; set; }
        public string QuestionOptionB { get; set; }
        public string QuestionOptionD { get; set; }
        public string QuestionOptionC { get; set; }

        //Answer
        public string Answer { get; set; }
        public string AnswerOptionA { get; set; }
        public string AnswerOptionB { get; set; }
        public string AnswerOptionC { get; set; }
        public string AnswerOptionD { get; set; }

        public DateTimeOffset ReleaseDate { get; set; }
    }
}
