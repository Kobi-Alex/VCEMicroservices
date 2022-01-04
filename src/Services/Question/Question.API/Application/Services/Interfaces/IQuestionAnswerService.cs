using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Question.API.Application.Contracts.Dtos.QuestionAnswerDtos;

namespace Question.API.Application.Services.Interfaces
{
    public interface IQuestionAnswerService
    {
        Task<IEnumerable<QuestionAnswerReadDto>> GetAllByCategoryIdAndQuestionIdAsync(int questionId, CancellationToken cancellationToken = default);
        Task<QuestionAnswerReadDto> GetByIdAsync(int categoryId, int questionId, int answerId, CancellationToken cancellationToken = default);
        Task<QuestionAnswerReadDto> CreateAsync(int categoryId, int questionId, QuestionAnswerCreateDto answerCreateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int categoryId, int questionId, int answerId, QuestionAnswerUpdateDto answerUpdateDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int categoryId, int questionId, int answerId, CancellationToken cancellationToken = default);
    }
}
