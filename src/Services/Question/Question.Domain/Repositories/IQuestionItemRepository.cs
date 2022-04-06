using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Question.Domain.Entities;
using System.Linq.Expressions;

namespace Question.Domain.Repositories
{
    // Question item repository interface
    public interface IQuestionItemRepository
    {
        Task<IEnumerable<QuestionItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<QuestionItem> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<QuestionItem> GetByIdIncludeAnswersAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<QuestionItem>> FindAll(Expression<Func<QuestionItem, bool>> predicate, CancellationToken cancellationToken = default);

        void Insert(QuestionItem item);
        void Remove(QuestionItem item);
        bool IsQuestionExists(int id);

    }
}
