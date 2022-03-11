using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Exam.Domain.Entities;
using Exam.Domain.Repositories;

namespace Exam.Infrastructure.Persistance.Repositories
{
    internal sealed class ExamItemRepository : IExamItemRepository
    {
        private readonly ExamDbContext _dbContext;

        public ExamItemRepository(ExamDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ExamItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Exams
                .Include(e => e.ExamQuestions)
                .ToListAsync(cancellationToken);
        }

        public async Task<ExamItem> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Exams
                .Include(e => e.ExamQuestions)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<ExamItem>> GetAllByStatusAsync(ExamStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Exams
                .Where(ei => ei.Status == status)
                .ToListAsync(cancellationToken);
        }

        public void Insert(ExamItem item)
        {
            _dbContext.Exams.Add(item);
        }

        public void Remove(ExamItem item)
        {
            _dbContext.Exams.Remove(item);
        }

        public bool IsExamItemExists(int id)
        {
            return _dbContext.Exams.Any(i => i.Id == id);
        }
    }
}
