using System;
using Question.Domain.Entities;
using System.Collections.Generic;

namespace Question.API.Application.Contracts.Dtos.QuestionCategoryDtos
{
    public class QuestionCategoryReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<QuestionItem> QuestionItems { get; set; }
    }
}
