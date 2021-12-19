using Exam.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exam.Domain.Domain.Repositories
{
    public interface IExamRepository
    {
        Task<IEnumerable<ExamItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ExamItem> GetByIdAsync(int examId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExamItem>> GetExamByTitleAsync(string title, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExamItem>> GetExamByCategoryAsync(string category, CancellationToken cancellationToken = default);
       
        void Insert(ExamItem exam);
        void Remove(ExamItem exam);
    }
}
