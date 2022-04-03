using System;
using System.ComponentModel.DataAnnotations;

namespace Applicant.API.Application.Contracts.Dtos.UserDtos
{
    public class UserExamDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "ExamId is required")]
        public int ExamId { get; set; }
    }
}
