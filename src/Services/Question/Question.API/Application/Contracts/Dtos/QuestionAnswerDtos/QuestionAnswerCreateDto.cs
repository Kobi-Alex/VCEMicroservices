using System;
using System.ComponentModel.DataAnnotations;

namespace Question.API.Application.Contracts.Dtos.QuestionAnswerDtos
{
    public class QuestionAnswerCreateDto
    {
        [Required(ErrorMessage = "Answer context is required")]
        [StringLength(400, MinimumLength = 3, ErrorMessage = "Answer context can't be longer than 400 characters")]
        public string Context { get; set; }

        [Range(0.0, 1.0)]
        public decimal CorrectAnswerCoefficient { get; set; }
    }
}
