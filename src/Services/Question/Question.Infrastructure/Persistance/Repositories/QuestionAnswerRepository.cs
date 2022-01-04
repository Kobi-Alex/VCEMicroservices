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
    internal sealed class QuestionAnswerRepository : IQuestionAnswerRepository
    {
        private readonly QuestionDbContext _dbContext;

        public QuestionAnswerRepository(QuestionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<QuestionAnswer>> GetAllByCategoryIdAndQuestionIdAsync(int questionItemId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Answers
                .Where(q => q.QuestionItemId == questionItemId)
                .ToListAsync(cancellationToken);
        }

        public async Task<QuestionAnswer> GetByIdAsync(int questionAnswerId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Answers
                .FirstOrDefaultAsync(a => a.Id == questionAnswerId, cancellationToken);
        }

        public void Insert(QuestionAnswer answer)
        {
            _dbContext.Answers.Add(answer);
        }

        public void Remove(QuestionAnswer answer)
        {
            _dbContext.Answers.Remove(answer);
        }
    }
}
