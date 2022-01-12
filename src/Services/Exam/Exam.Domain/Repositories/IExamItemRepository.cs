using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Exam.Domain.Entities;

namespace Exam.Domain.Repositories
{
    public interface IExamItemRepository
    {
        Task<IEnumerable<ExamItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ExamItem> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExamItem>> GetAllByStatusAsync(ExamStatus status, CancellationToken cancellationToken = default);


        void Insert(ExamItem item);
        void Remove(ExamItem item);
        bool IsExamItemExists(int id);
    }
}
