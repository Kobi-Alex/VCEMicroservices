using System;
using System.Collections.Generic;

namespace Question.Domain.Entities
{
    public class QuestionCategory
    {
        // Category question identity
        public int Id { get; set; }
        // Category name
        public string Name { get; set; }

        // List of questions
        public virtual ICollection<QuestionItem> QuestionItems { get; set; }
    }
}