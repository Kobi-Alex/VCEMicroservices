using Exam.Domain.Domain.Entities;
using Exam.Domain.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exam.Infrastructure.Persistance.Repositories
{
    internal sealed class ExamRepository : IExamRepository
    {
        private readonly ExamDbContext _dbContext;

        public ExamRepository(ExamDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ExamItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Exams.ToListAsync(cancellationToken);
        }

        public async Task<ExamItem> GetByIdAsync(int examId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Exams.FirstOrDefaultAsync(x => x.Id == examId, cancellationToken);
        }

        public async Task<IEnumerable<ExamItem>> GetExamByCategoryAsync(string category, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Exams.Where(x => x.Category == category).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ExamItem>> GetExamByTitleAsync(string title, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Exams.Where(x => x.Title == title).ToListAsync(cancellationToken);
        }

        public void Insert(ExamItem exam)
        {
            _dbContext.Exams.Add(exam);
        }

        public void Remove(ExamItem exam)
        {
            _dbContext.Exams.Remove(exam);
        }
    }
}
