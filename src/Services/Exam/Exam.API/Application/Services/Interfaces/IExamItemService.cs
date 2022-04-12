using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Exam.API.Application.Contracts.ExamItemDtos;


namespace Exam.API.Application.Services.Interfaces
{
    public interface IExamItemService
    {
        Task<IEnumerable<ExamItemReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ExamItemReadDto> GetByIdAsync(int examId, CancellationToken cancellationToken = default);
        Task<ExamItemReadDto> GetByIdIncludeExamQuestionsAsync(int examId, CancellationToken cancellationToken = default);


        Task<ExamItemReadDto> CreateAsync(ExamItemCreateDto examCreateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int examId, ExamItemUpdateDto examUpdateDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int examId, CancellationToken cancellationToken = default);
    }
}
