using Question.API.Application.Contracts.Dtos.QuestionCategoryDtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Question.API.Application.Services.Interfaces
{
    public interface IQuestionCategoryService
    {
        Task<IEnumerable<QuestionCategoryReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<QuestionCategoryReadDto> GetByIdAsync(int categoryId, CancellationToken cancellationToken = default);
        Task<QuestionCategoryReadDto> CreateAsync(QuestionCategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int categoryId, QuestionCategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default);
    }
}