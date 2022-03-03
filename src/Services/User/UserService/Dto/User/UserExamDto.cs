using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dto.User
{
    public class UserExamDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "ExamId is required")]
        public int ExamId { get; set; }
    }
}
