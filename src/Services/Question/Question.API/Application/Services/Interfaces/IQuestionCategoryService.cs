using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;
using Question.API.Application.Contracts.Dtos.QuestionCategoryDtos;

namespace Question.API.Application.Services.Interfaces
{
    public interface IQuestionCategoryService
    {
        Task<IEnumerable<QuestionCategoryReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<QuestionCategoryReadDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<QuestionCategoryReadDto> CreateAsync(QuestionCategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int id, QuestionCategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default);
    }
}