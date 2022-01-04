using Question.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Question.Domain.Repositories
{
    public interface IQuestionCategoryRepository
    {
        Task<IEnumerable<QuestionCategory>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<QuestionCategory> GetByIdAsync(int questionCategoryId, CancellationToken cancellationToken = default);

        void Insert(QuestionCategory category);
    }
}
