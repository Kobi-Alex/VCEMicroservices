using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Exam.Domain.Entities;

namespace Exam.Domain.Repositories
{
    public interface IExamQuestionRepository
    {
        Task<IEnumerable<ExamQuestion>> GetAllByExamItemAsync(int examId, CancellationToken cancellationToken = default);
        Task<ExamQuestion> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        void Insert(ExamQuestion item);
        void Remove(ExamQuestion item);
    }

}
