using System;


namespace Applicant.Domain.Entities
{
    public class UserExams
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ExamId { get; set; }
    }

}
