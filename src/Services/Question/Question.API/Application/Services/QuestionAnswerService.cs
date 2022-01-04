using AutoMapper;
using Question.API.Application.Contracts.Dtos.QuestionAnswerDtos;
using Question.API.Application.Services.Interfaces;
using Question.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

internal sealed class QuestionAnswerService : IQuestionAnswerService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public QuestionAnswerService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }


    public async Task<IEnumerable<QuestionAnswerReadDto>> GetAllByCategoryIdAndQuestionIdAsync(int questionId, CancellationToken cancellationToken = default)
    {
        var answers = await _repositoryManager.QuestionAnswerRepository.GetAllByCategoryIdAndQuestionIdAsync(questionId, cancellationToken);
        var answersDto = _mapper.Map<IEnumerable<QuestionAnswerReadDto>>(answers);

        return answersDto;
    }

    public Task<QuestionAnswerReadDto> GetByIdAsync(int categoryId, int questionId, int answerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<QuestionAnswerReadDto> CreateAsync(int categoryId, int questionId, QuestionAnswerCreateDto answerCreateDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int categoryId, int questionId, int answerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }


    public Task UpdateAsync(int categoryId, int questionId, int answerId, QuestionAnswerUpdateDto answerUpdateDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
