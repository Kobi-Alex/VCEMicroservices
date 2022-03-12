using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Question.Domain.Entities;


namespace Question.Domain.Repositories
{
    // Question category repository interface
    public interface IQuestionCategoryRepository
    {
        Task<IEnumerable<QuestionCategory>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<QuestionCategory> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        void Insert(QuestionCategory item);
        bool IsCategoryExists(int id);

    }
}
