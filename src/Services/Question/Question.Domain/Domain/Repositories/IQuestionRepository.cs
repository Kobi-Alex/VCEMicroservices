
using Question.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Question.Domain.Domain.Repositories
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<QuestionItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<QuestionItem> GetByIdAsync(int questionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<QuestionItem>> GetQuestionByCategory(string category, CancellationToken cancellationToken = default);

        void Insert(QuestionItem question);
        void Remove(QuestionItem question);
    }
}
