using System;


namespace Question.API.Application.Contracts.Dtos.QuestionAnswerDtos
{
    public class QuestionAnswerReadDto
    {
        public int Id { get; set; }
        public string CharKey { get; set; }
        public string Context { get; set; }
        public bool IsCorrectAnswer { get; set; }

        public int QuestionItemId { get; set; }
    }
}