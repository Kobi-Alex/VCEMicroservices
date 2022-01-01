using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Domain.Entities
{
    public class QuestionItem
    {
        public int Id { get; set; }
        public string Context { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }

        public QuestionCategory QuestionCategory { get; set; } 
        ICollection<QuestionAnswer> QuestionAnswers { get; set; }


    }

    //class Answer
    //{
    //    public int Id { get; set; }
    //    public int QuestionId { get; set; }
    //    public string Title { get; set; }
    //    public decimal CorrectAnswer { get; set; } 0.6
    //} 

}