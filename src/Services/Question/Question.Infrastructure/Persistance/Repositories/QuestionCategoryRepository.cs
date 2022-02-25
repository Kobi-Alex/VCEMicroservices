using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Question.Domain.Entities;
using Question.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Question.Infrastructure.Persistance.Repositories
{
    internal sealed class QuestionCategoryRepository : IQuestionCategoryRepository
    {
        private readonly QuestionDbContext _dbContext;

        public QuestionCategoryRepository(QuestionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<QuestionCategory>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<QuestionCategory> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public void Insert(QuestionCategory item)
        {
            _dbContext.Categories.Add(item);
        }

        public bool IsCategoryExists(int id)
        {
            return _dbContext.Categories.Any(e => e.Id == id);
        }
    }
}