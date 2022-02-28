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
    internal sealed class QuestionAnswerRepository : IQuestionAnswerRepository
    {
        private readonly QuestionDbContext _dbContext;

        public QuestionAnswerRepository(QuestionDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // Get all answers
        public async Task<IEnumerable<QuestionAnswer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Answers
                .ToListAsync(cancellationToken);
        }

        // Get all answers by question ID 
        public async Task<IEnumerable<QuestionAnswer>> GetAllByQuestionItemIdAsync(int questionId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Answers
                .Where(q => q.QuestionItemId == questionId)
                .ToListAsync(cancellationToken);
        }

        // Get question by ID
        public async Task<QuestionAnswer> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Answers
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        // Add new answer
        public void Insert(QuestionAnswer item)
        {
            _dbContext.Answers.Add(item);
        }

        // Remove answer
        public void Remove(QuestionAnswer item)
        {
            _dbContext.Answers.Remove(item);
        }
    }
}