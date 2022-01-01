using Question.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Question.Domain.Domain.Repositories
{
    public interface IQuestionAnswerRepository
    {
        Task<IEnumerable<QuestionAnswer>> GetAllByQuestionItemIdAsync(int questionItemId, CancellationToken cancellationToken = default);
        Task<QuestionAnswer> GetByIdAsync(int questionAnswerId, CancellationToken cancellationToken = default);

        void Insert(QuestionAnswer answer);
        void Remove(QuestionAnswer answer);
    }
}