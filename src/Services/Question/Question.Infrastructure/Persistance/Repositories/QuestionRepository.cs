using Microsoft.EntityFrameworkCore;
using Question.Domain.Domain.Entities;
using Question.Domain.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Question.Infrastructure.Persistance.Repositories
{
    internal sealed class QuestionRepository : IQuestionItemRepository
    {
        private readonly QuestionDbContext _dbContext;

        public QuestionRepository(QuestionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<QuestionItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions.ToListAsync(cancellationToken);
        }

        public async Task<QuestionItem> GetByIdAsync(int questionId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);
        }

        public async Task<IEnumerable<QuestionItem>> GetQuestionByCategory(string category, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions.Where(q => q.Category == category).ToListAsync(cancellationToken);
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
