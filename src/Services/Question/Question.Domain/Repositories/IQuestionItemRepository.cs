
using Question.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Question.Domain.Repositories
{
    public interface IQuestionItemRepository
    {
        Task<IEnumerable<QuestionItem>> GetAllByQuestionCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
        Task<QuestionItem> GetByIdAsync(int id, CancellationToken cancellationToken = default);
       
        void Insert(QuestionItem item);
        void Remove(QuestionItem item);
        bool IsQuestionExists(int id);
    }
}
