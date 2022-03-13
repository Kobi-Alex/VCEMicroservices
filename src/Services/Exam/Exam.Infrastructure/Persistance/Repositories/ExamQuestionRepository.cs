using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Exam.Domain.Entities;
using Exam.Domain.Repositories;

using Microsoft.EntityFrameworkCore;


namespace Exam.Infrastructure.Persistance.Repositories
{
    public sealed class ExamQuestionRepository : IExamQuestionRepository
    {
        private readonly ExamDbContext _dbContext;

        public ExamQuestionRepository(ExamDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<ExamQuestion>> GetAllByExamItemAsync(int examId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Where(q => q.ExamItemId == examId)
                .ToListAsync(cancellationToken);
        }

        public async Task<ExamQuestion> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

        public void Insert(ExamQuestion item)
        {
            _dbContext.Questions.Add(item);
        }

        public void Remove(ExamQuestion item)
        {
            _dbContext.Questions.Remove(item);
        }
    }
}
