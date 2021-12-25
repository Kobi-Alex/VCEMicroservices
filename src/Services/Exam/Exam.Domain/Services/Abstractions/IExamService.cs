using Exam.Domain.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exam.Domain.Services.Abstractions
{
    public interface IExamService
    {
        Task<IEnumerable<ExamReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ExamReadDto> GetByIdAsync(int examId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExamReadDto>> GetExamByTitleAsync(string title, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExamReadDto>> GetExamByCategoryAsync(string category, CancellationToken cancellationToken = default);

        Task<ExamReadDto> CreateAsync(ExamCreateDto examCreateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int examId, ExamUpdateDto examUpdateDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int examId, CancellationToken cancellationToken = default);
    }
}
