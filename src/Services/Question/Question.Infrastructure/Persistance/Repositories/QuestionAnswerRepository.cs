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

        public async Task<IEnumerable<QuestionAnswer>> GetAllByQuestionItemIdAsync(int questionId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Answers
                .Where(q => q.QuestionItemId == questionId)
                .ToListAsync(cancellationToken);
        }


        public async Task<QuestionAnswer> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Answers
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public void Insert(QuestionAnswer item)
        {
            _dbContext.Answers.Add(item);
        }

        public void Remove(QuestionAnswer item)
        {
            _dbContext.Answers.Remove(item);
        }
    }
}
