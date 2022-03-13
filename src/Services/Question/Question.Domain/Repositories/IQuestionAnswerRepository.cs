using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Question.Domain.Entities;


namespace Question.Domain.Repositories
{
    // Question answer repository interface
    public interface IQuestionAnswerRepository
    {
        Task<IEnumerable<QuestionAnswer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<QuestionAnswer>> GetAllByQuestionItemIdAsync(int questionId, CancellationToken cancellationToken = default);
        Task<QuestionAnswer> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        void Insert(QuestionAnswer item);
        void Remove(QuestionAnswer item);

    }
}