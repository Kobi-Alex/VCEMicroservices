using System;
using System.Collections.Generic;
using Question.Domain.Entities;

namespace Question.API.Application.Contracts.Dtos.QuestionItemDtos
{
    public class QuestionItemReadDto
    {
        public int Id { get; set; }
        public string Context { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }

        public int QuestionCategoryId { get; set; }
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }
    }
}
