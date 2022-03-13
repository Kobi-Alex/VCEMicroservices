using System;
using System.ComponentModel.DataAnnotations;

namespace Exam.API.Application.Contracts.ExamQuestionDtos
{
    public class ExamQuestionCreateDto
    {
        [Range(1, Int64.MaxValue, ErrorMessage = "The field {0} must be greater than or equal {1}.")]
        public int QuestionItemId { get; set; }
    }

}