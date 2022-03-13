using System;
using System.Collections.Generic;


namespace Question.Domain.Entities
{
    public class QuestionItem
    {
        // Question identity
        public int Id { get; set; }
        // Question context
        public string Context { get; set; }
        // Question release date
        public DateTimeOffset ReleaseDate { get; set; }
        // Question answer type
        public AnswerType AnswerType { get; set; }


        // Foreign Key
        public int QuestionCategoryId { get; set; }
        // Navigation property
        public virtual QuestionCategory QuestionCategory { get; set; }
        //List of answers
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
    }
}