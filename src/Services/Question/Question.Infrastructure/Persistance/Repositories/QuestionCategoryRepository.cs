using Microsoft.EntityFrameworkCore;
using Question.Domain.Entities;
using Question.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                .Include(c => c.QuestionItems)
                .ThenInclude(q => q.QuestionAnswers)
                .ToListAsync(cancellationToken);
        }

        public async Task<QuestionCategory> GetByIdAsync(int questionCategoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .Include(c => c.QuestionItems)
                .ThenInclude(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(x => x.Id == questionCategoryId, cancellationToken);
        }

        public void Insert(QuestionCategory category)
        {
            _dbContext.Categories.Add(category);
        }
    }
}