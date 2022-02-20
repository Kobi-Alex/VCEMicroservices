using System;

namespace Question.Domain.Entities
{
    public class QuestionAnswer
    {
        //Answer Identity
        public int Id { get; set; }
        // Char key - current sign answer (A or B or C or D or E).
        // For one question and empty text field for answer, CharKey = T (T- as text answer).
        public string CharKey { get; set; }
        //Answer context
        public string Context { get; set; }
        // Correct answer or No
        public bool IsCorrectAnswer { get; set; }
        

        // Foreign Key
        public int QuestionItemId { get; set; }
        // Navigation property
        public virtual QuestionItem QuestionItem { get; set; }
    }
}