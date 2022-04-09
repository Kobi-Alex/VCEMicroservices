using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Applicant.Domain.Entities;


namespace Applicant.Domain.Repositories
{
    public interface IUserExamsRepository
    {
        Task<IEnumerable<UserExams>> GetAllAsync(string id);
        Task<UserExams> GetByUserIdAndExamId(string userId, int examId);

        Task<IEnumerable<UserExams>> GetAllUsersByExamId(int examId);

        void Insert(UserExams userExam);
        void Remove(UserExams userExam);
    }

}
