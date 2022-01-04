using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;
using Question.API.Application.Services.Interfaces;
using Question.Domain.Repositories;

namespace Question.API.Application.Services
{
    internal sealed class QuestionItemService : IQuestionItemService
    {
        private IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public QuestionItemService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuestionItemReadDto>> GetAllByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var questions = await _repositoryManager.QuestionItemRepository.GetAllByCategoryIdAsync(categoryId, cancellationToken);
            var questionsDto = _mapper.Map<IEnumerable<QuestionItemReadDto>>(questions);

            return questionsDto;
        }

        public Task<QuestionItemReadDto> GetByIdAsync(int categoryId, int questionId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<QuestionItemReadDto> CreateAsync(int categoryId, QuestionItemCreateDto questionCreateDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int categoryId, int questionId, QuestionItemUpdateDto questionUpdateDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int categoryId, int questionId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }



        //public async Task<IEnumerable<QuestionItemReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        //{
        //    var questions = await _repositoryManager.QuestionItemRepository.GetAllAsync(cancellationToken);
        //    var questionDto = _mapper.Map<IEnumerable<QuestionItemReadDto>>(questions);

        //    return questionDto;
        //}

        //public async Task<QuestionItemReadDto> GetByIdAsync(int questionItemId, CancellationToken cancellationToken = default)
        //{
        //    var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionItemId, cancellationToken);

        //    if (question is null)
        //    {
        //        throw new QuestionItemNotFoundException(questionItemId);
        //    }

        //    var questionDto = _mapper.Map<QuestionItemReadDto>(question);

        //    return questionDto;
        //}


        //public async Task<QuestionItemReadDto> CreateAsync(QuestionItemCreateDto questionCreateDto, CancellationToken cancellationToken = default)
        //{
        //    if (questionCreateDto is null)
        //    {
        //        throw new ArgumentNullException(nameof(questionCreateDto));
        //    }

        //    var question = _mapper.Map<QuestionItem>(questionCreateDto);

        //    _repositoryManager.QuestionItemRepository.Insert(question);

        //    await _repositoryManager.UnitOfWork.SaveChangesAsync();

        //    return _mapper.Map<QuestionItemReadDto>(question);
        //}


        //public async Task UpdateAsync(int questionItemId, QuestionItemUpdateDto questionUpdateDto, CancellationToken cancellationToken = default)
        //{
        //    var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionItemId);

        //    if (question is null)
        //    {
        //        throw new QuestionItemNotFoundException(questionItemId);
        //    }


        //    question.Context = questionUpdateDto.Context;
        //    question.ReleaseDate = questionUpdateDto.ReleaseDate;

        //    await _repositoryManager.UnitOfWork.SaveChangesAsync();
        //}

        //public async Task DeleteAsync(int questionId, CancellationToken cancellationToken = default)
        //{
        //    var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionId);

        //    if (question is null)
        //    {
        //        throw new QuestionItemNotFoundException(questionId);
        //    }

        //    _repositoryManager.QuestionItemRepository.Remove(question);

        //    await _repositoryManager.UnitOfWork.SaveChangesAsync();
        //}

    }
}