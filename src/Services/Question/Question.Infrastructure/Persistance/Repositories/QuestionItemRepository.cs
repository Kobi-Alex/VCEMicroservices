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
    internal sealed class QuestionItemRepository : IQuestionItemRepository
    {
        private readonly QuestionDbContext _dbContext;

        public QuestionItemRepository(QuestionDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<QuestionItem>> GetAllByQuestionCategoryIdAsync(int questionCategoryId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Where(q => q.QuestionCategoryId == questionCategoryId)
                .Include(q => q.QuestionAnswers)
                .ToListAsync(cancellationToken);
        }

        public async Task<QuestionItem> GetByIdAsync(int questionItemId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Include(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(q => q.Id == questionItemId, cancellationToken);
        }

        public void Insert(QuestionItem question)
        {
            _dbContext.Questions.Add(question);
        }

        public void Remove(QuestionItem question)
        {
            _dbContext.Questions.Remove(question);
        }
    }
}
