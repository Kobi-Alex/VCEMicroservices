using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Exam.API.Application.Contracts.ExamQuestionDtos;

namespace Exam.API.Application.Services.Abstractions
{
    public interface IExamQuestionService
    {
        Task<IEnumerable<ExamQuestionReadDto>> GetAllByExamItemIdAsync(int examId, CancellationToken cancellationToken = default);
        Task<ExamQuestionReadDto> GetByIdAsync(int examId, int questionId, CancellationToken cancellationToken);
        Task<ExamQuestionReadDto> CreateAsync(int examId, ExamQuestionCreateDto examQuestionCreateDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int examId, int questionId, CancellationToken cancellationToken = default);
    }
}
