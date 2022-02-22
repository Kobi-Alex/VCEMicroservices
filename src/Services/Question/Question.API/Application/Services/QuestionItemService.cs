using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Question.Domain.Repositories;
using Question.API.Application.Services.Interfaces;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;
using AutoMapper;
using MassTransit;
using Question.Domain.Entities;
using Question.API.Application.Exceptions;
using Exam.API.Application.IntegrationEvents.Events;

namespace Question.API.Application.Services
{
    internal sealed class QuestionItemService : IQuestionItemService
    {
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepositoryManager _repositoryManager;

        public QuestionItemService(IRepositoryManager repositoryManager, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _repositoryManager = repositoryManager;
        }



        public async Task<IEnumerable<QuestionItemReadDto>> GetAllByQuestionCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {

            if (! _repositoryManager.QuestionCategoryRepository.IsCategoryExists(categoryId))
            {
                throw new QuestionCategoryNotFoundException(categoryId);
            }

            var questions = await _repositoryManager.QuestionItemRepository.GetAllByQuestionCategoryIdAsync(categoryId, cancellationToken);
            var questionsDto = _mapper.Map<IEnumerable<QuestionItemReadDto>>(questions);

            return questionsDto;
        }


        public async Task<QuestionItemReadDto> GetByIdAsync(int categoryId, int questionId,  CancellationToken cancellationToken = default)
        {

            if (!_repositoryManager.QuestionCategoryRepository.IsCategoryExists(categoryId))
            {
                throw new QuestionCategoryNotFoundException(categoryId);
            }

            var question = await GetQuestinItemInCurrentDirectory(categoryId, questionId, cancellationToken);
            var questionDto = _mapper.Map<QuestionItemReadDto>(question);

            return questionDto;
        }


        public async Task<QuestionItemReadDto> CreateAsync(int categoryId, QuestionItemCreateDto questionItemCreateDto, CancellationToken cancellationToken = default)
        {
            var category = await _repositoryManager.QuestionCategoryRepository.GetByIdAsync(categoryId, cancellationToken);

            if (category is null)
            {
                throw new QuestionCategoryNotFoundException(categoryId);
            }

            var question = _mapper.Map<QuestionItem>(questionItemCreateDto);

            question.ReleaseDate = new DateTimeOffset(DateTime.Now);
            question.QuestionCategoryId = category.Id;

            _repositoryManager.QuestionItemRepository.Insert(question);
            await _repositoryManager.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<QuestionItemReadDto>(question);
        }


        public async Task UpdateAsync(int categoryId, int questionId, QuestionItemUpdateDto questionUpdateDto, CancellationToken cancellationToken = default)
        {

            if (!_repositoryManager.QuestionCategoryRepository.IsCategoryExists(categoryId))
            {
                throw new QuestionCategoryNotFoundException(categoryId);
            }

            var question = await GetQuestinItemInCurrentDirectory(categoryId, questionId, cancellationToken);

            question.Context = questionUpdateDto.Context;
            question.AnswerType = question.AnswerType;

            await _repositoryManager.UnitOfWork.SaveChangesAsync();
        }


        public async Task DeleteAsync(int categoryId, int questionId, CancellationToken cancellationToken = default)
        {

            if (!_repositoryManager.QuestionCategoryRepository.IsCategoryExists(categoryId))
            {
                throw new QuestionCategoryNotFoundException(categoryId);
            }

            var question = await GetQuestinItemInCurrentDirectory(categoryId, questionId, cancellationToken);

            //Event (send messaga to RubbitMQ server)
            var eventMessage = _mapper.Map<QuestionItemDeleteEvent>(question);
            await _publishEndpoint.Publish(eventMessage);

            _repositoryManager.QuestionItemRepository.Remove(question);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        }

        private async Task<QuestionItem> GetQuestinItemInCurrentDirectory(int categoryId, int questionId, CancellationToken cancellationToken = default)
        {
            var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionId, cancellationToken);

            if (question is null)
            {
                throw new QuestionItemNotFoundException(questionId);
            }

            if (question.QuestionCategoryId != categoryId)
            {
                throw new QuestionItemDoesNotBelongToQuestionCategoryException(categoryId, questionId);
            }

            return question;
        }

    }
}