using Exam.API.Application.Contracts.ExamItemDtos;
using Exam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exam.API.Application.Services.Abstractions
{
    public interface IExamItemService
    {
        Task<IEnumerable<ExamItemReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ExamItemReadDto>> GetAllByStatusAsync(ExamStatus status, CancellationToken cancellationToken = default);
        Task<ExamItemReadDto> GetByIdAsync(int examId, CancellationToken cancellationToken = default);


        Task<ExamItemReadDto> CreateAsync(ExamItemCreateDto examCreateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int examId, ExamItemUpdateDto examUpdateDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int examId, CancellationToken cancellationToken = default);
    }
}
