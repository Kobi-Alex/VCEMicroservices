using System;
using System.ComponentModel.DataAnnotations;
using Question.Domain.Entities;

namespace Question.API.Application.Contracts.Dtos.QuestionItemDtos
{
    public class QuestionItemCreateDto
    {

        //Question
        [Required(ErrorMessage = "Question context is required")]
        [StringLength(400, MinimumLength = 3, ErrorMessage = "Question context can't be longer than 400 characters and can't be less 3 characters")]
        public string Context { get; set; }

        //Date
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTimeOffset ReleaseDate { get; set; }

        //Answer type
        [Required(ErrorMessage = "Question answer type is required")]
        public AnswerType AnswerType { get; set; }

    }
}
