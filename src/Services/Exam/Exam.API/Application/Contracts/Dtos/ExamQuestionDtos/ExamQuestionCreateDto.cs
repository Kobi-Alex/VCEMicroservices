using System;
using System.ComponentModel.DataAnnotations;

namespace Exam.API.Application.Contracts.ExamQuestionDtos
{
    public class ExamQuestionCreateDto
    {
        [Required(ErrorMessage = "Question context is required")]
        public string Question { get; set; }
    }

}