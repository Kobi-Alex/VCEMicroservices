using System;
using System.ComponentModel.DataAnnotations;

namespace Question.API.Application.Contracts.Dtos.QuestionAnswerDtos
{
    public class QuestionAnswerCreateDto
    {
        [Required(ErrorMessage = "Answer char key is required")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Char key can't be longer than 1 character")]
        public string CharKey { get; set; }

        [Required(ErrorMessage = "Answer context is required")]
        [StringLength(400, MinimumLength = 3, ErrorMessage = "Answer context can't be longer than 400 characters and can't be less 3 characters")]
        public string Context { get; set; }

        public bool IsCorrectAnswer { get; set; }
        public int QuestionItemId { get; set; }
    }
}
