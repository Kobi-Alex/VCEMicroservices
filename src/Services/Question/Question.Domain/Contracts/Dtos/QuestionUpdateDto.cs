using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Contracts.Dtos
{
    public class QuestionUpdateDto
    {
        [Required(ErrorMessage = "Category is required")]
        [StringLength(30, ErrorMessage = "Category can't be longer than 30 characters")]
        public string Category { get; set; }


        //Question
        [Required(ErrorMessage = "Question is required")]
        [StringLength(200, ErrorMessage = "Question can't be longer than 200 characters")]
        public string Question { get; set; }

        [StringLength(200, ErrorMessage = "Question A can't be longer than 200 characters")]
        public string QuestionOptionA { get; set; }

        [StringLength(200, ErrorMessage = "Question B can't be longer than 200 characters")]
        public string QuestionOptionB { get; set; }

        [StringLength(200, ErrorMessage = "Question C can't be longer than 200 characters")]
        public string QuestionOptionD { get; set; }

        [StringLength(200, ErrorMessage = "Question D can't be longer than 200 characters")]
        public string QuestionOptionC { get; set; }


        //Answer
        [Required(ErrorMessage = "Answer is required")]
        [StringLength(200, ErrorMessage = "Answer can't be longer than 200 characters")]
        public string Answer { get; set; }

        [StringLength(200, ErrorMessage = "Answer A can't be longer than 200 characters")]
        public string AnswerOptionA { get; set; }

        [StringLength(200, ErrorMessage = "Answer B can't be longer than 200 characters")]
        public string AnswerOptionB { get; set; }

        [StringLength(200, ErrorMessage = "Answer C can't be longer than 200 characters")]
        public string AnswerOptionC { get; set; }

        [StringLength(200, ErrorMessage = "Answer D can't be longer than 200 characters")]
        public string AnswerOptionD { get; set; }
    }
}
