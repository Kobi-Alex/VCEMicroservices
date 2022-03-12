using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Question.API.Application.Contracts.Dtos.QuestionAnswerDtos;


namespace Question.API.Application.Services.Interfaces
{
    // Answer service interface
    public interface IQuestionAnswerService
    {
        Task<IEnumerable<QuestionAnswerReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<QuestionAnswerReadDto>> GetAllByQuestionItemIdAsync(int questionId, CancellationToken cancellationToken = default);
        Task<QuestionAnswerReadDto> GetByIdAsync(int answerId, CancellationToken cancellationToken = default);
        Task<QuestionAnswerReadDto> CreateAsync(QuestionAnswerCreateDto answerCreateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int answerId, QuestionAnswerUpdateDto answerUpdateDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int answerId, CancellationToken cancellationToken = default);

    }
}
