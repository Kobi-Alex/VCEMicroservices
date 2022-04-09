using System;
using System.Linq;
using System.Threading.Tasks;
using Applicant.Domain.Entities;
using System.Collections.Generic;
using Applicant.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Applicant.Infrasructure.Persistance.Repositories
{
    internal sealed class UserExamsRepository : IUserExamsRepository
    {

        private readonly AppDbContext _dbContext;
        public UserExamsRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }


        public async Task<IEnumerable<UserExams>> GetAllAsync(string id)
        {
            return await _dbContext.UserExams
                .Where(x => x.UserId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserExams>> GetAllUsersByExamId(int examId)
        {
            return await _dbContext.UserExams.Include(u=>u.User).Where(x => x.ExamId == examId).ToListAsync();
        }

        public async Task<UserExams> GetByUserIdAndExamId(string userId, int examId)
        {
            return await _dbContext.UserExams
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ExamId == examId);
        }

        public void Insert(UserExams userExam)
        {
            _dbContext.UserExams.Add(userExam);
        }

        public void Remove(UserExams userExam)
        {
            _dbContext.UserExams.Remove(userExam);
        }
    }
}
