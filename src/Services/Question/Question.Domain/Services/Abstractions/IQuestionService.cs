using Question.Domain.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Question.Domain.Services.Abstractions
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<QuestionReadDto> GetByIdAsync(int questionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<QuestionReadDto>> GetQuestionByCategoryAsync(string category, CancellationToken cancellationToken = default);
        Task<QuestionReadDto> CreateAsync(QuestionCreateDto questionCreateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int questionId, QuestionUpdateDto questionUpdateDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int questionId, CancellationToken cancellationToken = default);
    }
}
