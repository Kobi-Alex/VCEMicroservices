using System;
using System.ComponentModel.DataAnnotations;

namespace Question.API.Application.Contracts.Dtos.QuestionAnswerDtos
{
    public class QuestionAnswerUpdateDto
    {
        [Required(ErrorMessage = "Answer context is required")]
        [StringLength(400, ErrorMessage = "Answer context can't be longer than 400 characters")]
        public string Context { get; set; }


        [Required(ErrorMessage = "Correct answer coefficient is required")]
        public decimal CorrectAnswerCoefficient { get; set; }
    }
}