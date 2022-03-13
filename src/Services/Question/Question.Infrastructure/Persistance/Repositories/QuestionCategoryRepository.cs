using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Question.Domain.Entities;
using Question.Domain.Repositories;

using Microsoft.EntityFrameworkCore;


namespace Question.Infrastructure.Persistance.Repositories
{
    internal sealed class QuestionCategoryRepository : IQuestionCategoryRepository
    {
        private readonly QuestionDbContext _dbContext;

        public QuestionCategoryRepository(QuestionDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // Get all category
        public async Task<IEnumerable<QuestionCategory>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        // Get all category by ID
        public async Task<QuestionCategory> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        // Insert category
        public void Insert(QuestionCategory item)
        {
            _dbContext.Categories.Add(item);
        }

        // If category exists
        public bool IsCategoryExists(int id)
        {
            return _dbContext.Categories.Any(e => e.Id == id);
        }

    }
}