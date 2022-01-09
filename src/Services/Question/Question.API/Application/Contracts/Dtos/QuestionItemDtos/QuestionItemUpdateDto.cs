using System;
using System.ComponentModel.DataAnnotations;

namespace Question.API.Application.Contracts.Dtos.QuestionItemDtos
{
    public class QuestionItemUpdateDto
    {
        //Question
        [Required(ErrorMessage = "Question context is required")]
        [StringLength(400, MinimumLength = 3, ErrorMessage = "Question context can't be longer than 400 characters")]
        public string Context { get; set; }

    }
}