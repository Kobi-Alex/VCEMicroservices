using System;
using System.ComponentModel.DataAnnotations;

namespace Question.API.Application.Contracts.Dtos.QuestionItemDtos
{
    public class QuestionItemUpdateDto
    {
        //Question
        [Required(ErrorMessage = "Question context is required")]
        [StringLength(400, ErrorMessage = "Question context can't be longer than 400 characters")]
        public string Context { get; set; }

        //Date
        [Required(ErrorMessage = "Date of question release is required")]
        public DateTimeOffset ReleaseDate { get; set; }
    }
}
