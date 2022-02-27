using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Question.Domain.Entities;
using Question.Domain.Repositories;

namespace Question.Infrastructure.Persistance.Repositories
{
    internal sealed class QuestionItemRepository : IQuestionItemRepository
    {
        private readonly QuestionDbContext _dbContext;

        public QuestionItemRepository(QuestionDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // Get all questions
        public async Task<IEnumerable<QuestionItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .ToListAsync(cancellationToken);
        }

        // Get questions by ID
        public async Task<QuestionItem> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

        // Get questions by ID include answers
        public async Task<QuestionItem> GetByIdIncludeAnswersAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Include(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

        // Insert question
        public void Insert(QuestionItem item)
        {
            _dbContext.Questions.Add(item);
        }

        // Remove question
        public void Remove(QuestionItem item)
        {
            _dbContext.Questions.Remove(item);
        }

        // Check if question exist
        public bool IsQuestionExists(int id)
        {
            return _dbContext.Questions.Any(e => e.Id == id);
        }


        //public async Task<IEnumerable<QuestionItem>> GetAllByQuestionCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
        //{
        //    return await _dbContext.Questions
        //        .Where(q => q.QuestionCategoryId == categoryId)
        //        .ToListAsync(cancellationToken);    
        //}

    }
}
