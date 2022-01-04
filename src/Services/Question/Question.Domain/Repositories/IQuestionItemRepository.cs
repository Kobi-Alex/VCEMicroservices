
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
        Task<IEnumerable<QuestionItem>> GetAllByCategoryIdAsync(int questionCategoryId, CancellationToken cancellationToken = default);
        Task<QuestionItem> GetByIdAsync(int questionItemId, CancellationToken cancellationToken = default);
       
        void Insert(QuestionItem question);
        void Remove(QuestionItem question);
    }
}
