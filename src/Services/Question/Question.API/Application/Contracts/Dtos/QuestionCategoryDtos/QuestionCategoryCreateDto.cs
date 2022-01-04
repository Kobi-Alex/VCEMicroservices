using System;
using System.ComponentModel.DataAnnotations;

namespace Question.API.Application.Contracts.Dtos.QuestionCategoryDtos
{
    public class QuestionCategoryCreateDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(60, ErrorMessage = "Category name can't be longer than 60 characters")]
        public string Name { get; set; }
    }
}
