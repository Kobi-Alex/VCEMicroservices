using System;
using System.Threading;
using Exam.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;


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
