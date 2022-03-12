using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;


namespace Question.API.Application.Services.Interfaces
{
    // Question service interface
    public interface IQuestionItemService
    {
        Task<IEnumerable<QuestionItemReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<QuestionItemReadDto> GetByIdAsync(int questionId, CancellationToken cancellationToken = default);
        Task<QuestionItemReadDto> GetByIdIncludeAnswersAsync(int questionId, CancellationToken cancellationToken = default);
        Task<QuestionItemReadDto> CreateAsync(QuestionItemCreateDto questionCreateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int questionId, QuestionItemUpdateDto questionUpdateDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int questionId, CancellationToken cancellationToken = default);

    }
}