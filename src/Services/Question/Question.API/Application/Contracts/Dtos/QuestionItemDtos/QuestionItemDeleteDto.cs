using System;
using Question.Domain.Entities;
using System.Collections.Generic;


namespace Question.API.Application.Contracts.Dtos.QuestionItemDtos
{
    public class QuestionItemDeleteDto
    {
        public int Id { get; set; }
        public string Context { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public AnswerType AnswerType { get; set; }

        public int QuestionCategoryId { get; set; }
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }
    }
}
