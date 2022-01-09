using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Question.Domain.Entities;
using Question.Domain.Repositories;
using Question.API.Application.Exceptions;
using Question.API.Application.Services.Interfaces;
using Question.API.Application.Contracts.Dtos.QuestionAnswerDtos;
using AutoMapper;

namespace Question.API.Application.Services
{
    internal sealed class QuestionAnswerService : IQuestionAnswerService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public QuestionAnswerService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }


        public async Task<IEnumerable<QuestionAnswerReadDto>> GetAllByQuestionItemIdAsync(int categoryId, int questionId, CancellationToken cancellationToken = default)
        {

            await IsExistsQuestionItem(categoryId, questionId, cancellationToken);

            var answers = await _repositoryManager.QuestionAnswerRepository.GetAllByQuestionItemIdAsync(questionId, cancellationToken);
            var answersDto = _mapper.Map<IEnumerable<QuestionAnswerReadDto>>(answers);

            return answersDto;
        }


        public async Task<QuestionAnswerReadDto> GetByIdAsync(int categoryId, int questionId, int answerId, CancellationToken cancellationToken = default)
        {

            await IsExistsQuestionItem(categoryId, questionId, cancellationToken);

            var answer = await GetQuestinAnswerInCurrentDirectory(questionId, answerId, cancellationToken);

            var answerDto = _mapper.Map<QuestionAnswerReadDto>(answer);

            return answerDto;
        }


        public async Task<QuestionAnswerReadDto> CreateAsync(int categoryId, int questionId, QuestionAnswerCreateDto questionAnswerCreateDto, CancellationToken cancellationToken = default)
        {

            if (!_repositoryManager.QuestionCategoryRepository.IsCategoryExists(categoryId))
            {
                throw new QuestionCategoryNotFoundException(categoryId);
            }
                
            var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionId, cancellationToken);

            if (question is null)
            {
                throw new QuestionItemNotFoundException(questionId);
            }

            if (question.QuestionCategoryId != categoryId)
            {
                throw new QuestionItemDoesNotBelongToQuestionCategoryException(categoryId, questionId);
            }

            var answer = _mapper.Map<QuestionAnswer>(questionAnswerCreateDto);
            answer.QuestionItemId = question.Id;

            _repositoryManager.QuestionAnswerRepository.Insert(answer);
            await _repositoryManager.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<QuestionAnswerReadDto>(answer);
        }


        public async Task UpdateAsync(int categoryId, int questionId, int answerId, QuestionAnswerUpdateDto answerUpdateDto, CancellationToken cancellationToken = default)
        {

            await IsExistsQuestionItem(categoryId, questionId, cancellationToken);

            var answer = await GetQuestinAnswerInCurrentDirectory(questionId, answerId, cancellationToken);

            answer.Context = answerUpdateDto.Context;
            answer.CorrectAnswerCoefficient = answerUpdateDto.CorrectAnswerCoefficient;

            await _repositoryManager.UnitOfWork.SaveChangesAsync();
        }


        public async Task DeleteAsync(int categoryId, int questionId, int answerId, CancellationToken cancellationToken = default)
        {
            await IsExistsQuestionItem(categoryId, questionId, cancellationToken);

            var answer = await GetQuestinAnswerInCurrentDirectory(questionId, answerId, cancellationToken);

            _repositoryManager.QuestionAnswerRepository.Remove(answer);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }


        private async Task IsExistsQuestionItem(int categoryId, int questionId, CancellationToken cancellationToken = default)
        {
            if (!_repositoryManager.QuestionCategoryRepository.IsCategoryExists(categoryId))
            {
                throw new QuestionCategoryNotFoundException(categoryId);
            }

            var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionId, cancellationToken);

            if (question is null)
            {
                throw new QuestionItemNotFoundException(questionId);
            }

            if (question.QuestionCategoryId != categoryId)
            {
                throw new QuestionItemDoesNotBelongToQuestionCategoryException(categoryId, questionId);
            }
        }


        private async Task<QuestionAnswer> GetQuestinAnswerInCurrentDirectory(int questionId, int answerId, CancellationToken cancellationToken = default)
        {
            var answer = await _repositoryManager.QuestionAnswerRepository.GetByIdAsync(answerId, cancellationToken);

            if (answer is null)
            {
                throw new QuestionAnswerNotFoundException(answerId);
            }

            if (answer.QuestionItemId != questionId)
            {
                throw new QuestionAnswerDoesNotBelongToQuestionItemException(questionId, answerId);
            }

            return answer;
        }
    }
}