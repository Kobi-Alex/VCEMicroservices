using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Applicant.API.Application.Contracts.Dtos.UserDtos
{
    public class UserExamReadDto
    {
        public UserReadDto User { get; set; }
        public int ExamId { get; set; }
    }
}
