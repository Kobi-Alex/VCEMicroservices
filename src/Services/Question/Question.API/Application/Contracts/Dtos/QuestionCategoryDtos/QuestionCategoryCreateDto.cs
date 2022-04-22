using System;
using System.ComponentModel.DataAnnotations;


namespace Question.API.Application.Contracts.Dtos.QuestionCategoryDtos
{
    public class QuestionCategoryCreateDto
    {
        //[Required(ErrorMessage = "Category name is required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Category name can't be longer than 60 characters " +
            "and less than 3 characters")]
        public string Name { get; set; }
    }
}
