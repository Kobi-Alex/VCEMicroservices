using Question.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Question.Domain.Repositories
{
    public interface IQuestionAnswerRepository
    {

        Task<IEnumerable<QuestionAnswer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<QuestionAnswer>> GetAllByQuestionItemIdAsync(int questionId, CancellationToken cancellationToken = default);
        Task<QuestionAnswer> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        void Insert(QuestionAnswer item);
        void Remove(QuestionAnswer item);

    }
}