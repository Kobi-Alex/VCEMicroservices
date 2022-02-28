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
    // Question service
    // Service in which the interface IQuestionItemService and its methods are implemented
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



        // Get all questions from DB
        public async Task<IEnumerable<QuestionItemReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var questionItems = await _repositoryManager.QuestionItemRepository.GetAllAsync(cancellationToken);
            var questionItemsDto = _mapper.Map<IEnumerable<QuestionItemReadDto>>(questionItems);

            return questionItemsDto;
        }


        // Get question by ID from DB
        public async Task<QuestionItemReadDto> GetByIdAsync(int questionId, CancellationToken cancellationToken = default)
        {

            var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionId, cancellationToken);

            if(question is null)
            {
                throw new QuestionItemNotFoundException(questionId);
            }

            var questionDto = _mapper.Map<QuestionItemReadDto>(question);

            return questionDto;
        }


        // Get question by ID include list answers from DB
        public async Task<QuestionItemReadDto> GetByIdIncludeAnswersAsync(int questionId, CancellationToken cancellationToken = default)
        {
            var question = await _repositoryManager.QuestionItemRepository.GetByIdIncludeAnswersAsync(questionId, cancellationToken);

            if (question is null)
            {
                throw new QuestionItemNotFoundException(questionId);
            }

            var questionDto = _mapper.Map<QuestionItemReadDto>(question);

            return questionDto;
        }


        // Create new question
        public async Task<QuestionItemReadDto> CreateAsync(QuestionItemCreateDto questionCreateDto, CancellationToken cancellationToken = default)
        {

            if (questionCreateDto is null)
            {
                throw new QuestionItemArgumentException(nameof(questionCreateDto));
            }

            var category = await _repositoryManager.QuestionCategoryRepository
                .GetByIdAsync(questionCreateDto.QuestionCategoryId, cancellationToken);

            if (category is null)
            {
                throw new QuestionCategoryNotFoundException(questionCreateDto.QuestionCategoryId);
            }


            var question = _mapper.Map<QuestionItem>(questionCreateDto);

            question.ReleaseDate = new DateTimeOffset(DateTime.Now);

            _repositoryManager.QuestionItemRepository.Insert(question);
            await _repositoryManager.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<QuestionItemReadDto>(question);
        }


        // Update current question
        public async Task UpdateAsync(int questionId, QuestionItemUpdateDto questionUpdateDto, CancellationToken cancellationToken = default)
        {

            if (questionUpdateDto is null)
            {
                throw new QuestionItemArgumentException(nameof(questionUpdateDto));
            }

            if (!_repositoryManager.QuestionCategoryRepository
                .IsCategoryExists(questionUpdateDto.QuestionCategoryId))
            {
                throw new QuestionCategoryNotFoundException(questionUpdateDto.QuestionCategoryId);
            }


            var question = await _repositoryManager.QuestionItemRepository
                .GetByIdIncludeAnswersAsync(questionId);

            if(question is null)
            {
                throw new QuestionItemNotFoundException(questionId);
            }

            //TODO normal change answers
            if (question.AnswerType != questionUpdateDto.AnswerType)
            {
                question.QuestionAnswers.Clear();
            }

            question.Context = questionUpdateDto.Context;
            question.AnswerType = questionUpdateDto.AnswerType;
           
            await _repositoryManager.UnitOfWork.SaveChangesAsync();
        }


        // Delete current question
        public async Task DeleteAsync(int questionId, CancellationToken cancellationToken = default)
        {

            var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionId, cancellationToken);

            if (question is null)
            {
                throw new QuestionItemNotFoundException(questionId);
            }

            // Event (send messaga to RubbitMQ server)
            var eventMessage = _mapper.Map<QuestionItemDeleteEvent>(question);
            await _publishEndpoint.Publish(eventMessage);

            _repositoryManager.QuestionItemRepository.Remove(question);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Old code
        //public async Task<IEnumerable<QuestionItemReadDto>> GetAllByQuestionCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
        //{

        //    if (! _repositoryManager.QuestionCategoryRepository.IsCategoryExists(categoryId))
        //    {
        //        throw new QuestionCategoryNotFoundException(categoryId);
        //    }

        //    var questions = await _repositoryManager.QuestionItemRepository.GetAllByQuestionCategoryIdAsync(categoryId, cancellationToken);
        //    var questionsDto = _mapper.Map<IEnumerable<QuestionItemReadDto>>(questions);

        //    return questionsDto;
        //}


        //public async Task<QuestionItemReadDto> GetByIdAsync(int categoryId, int questionId,  CancellationToken cancellationToken = default)
        //{

        //    if (!_repositoryManager.QuestionCategoryRepository.IsCategoryExists(categoryId))
        //    {
        //        throw new QuestionCategoryNotFoundException(categoryId);
        //    }

        //    var question = await GetQuestinItemInCurrentDirectory(categoryId, questionId, cancellationToken);
        //    var questionDto = _mapper.Map<QuestionItemReadDto>(question);

        //    return questionDto;
        //}


        //public async Task<QuestionItemReadDto> CreateAsync(int categoryId, QuestionItemCreateDto questionItemCreateDto, CancellationToken cancellationToken = default)
        //{
        //    var category = await _repositoryManager.QuestionCategoryRepository.GetByIdAsync(categoryId, cancellationToken);

        //    if (category is null)
        //    {
        //        throw new QuestionCategoryNotFoundException(categoryId);
        //    }

        //    var question = _mapper.Map<QuestionItem>(questionItemCreateDto);

        //    question.ReleaseDate = new DateTimeOffset(DateTime.Now);
        //    question.QuestionCategoryId = category.Id;

        //    _repositoryManager.QuestionItemRepository.Insert(question);
        //    await _repositoryManager.UnitOfWork.SaveChangesAsync();

        //    return _mapper.Map<QuestionItemReadDto>(question);
        //}


        //public async Task UpdateAsync(int categoryId, int questionId, QuestionItemUpdateDto questionUpdateDto, CancellationToken cancellationToken = default)
        //{

        //    if (!_repositoryManager.QuestionCategoryRepository.IsCategoryExists(categoryId))
        //    {
        //        throw new QuestionCategoryNotFoundException(categoryId);
        //    }

        //    var question = await GetQuestinItemInCurrentDirectory(categoryId, questionId, cancellationToken);

        //    //TODO normal change answers
        //    if(question.AnswerType !=questionUpdateDto.AnswerType)
        //    {
        //        question.QuestionAnswers.Clear();
        //    }

        //    question.Context = questionUpdateDto.Context;
        //    question.AnswerType = questionUpdateDto.AnswerType;

        //    await _repositoryManager.UnitOfWork.SaveChangesAsync();
        //}


        //public async Task DeleteAsync(int categoryId, int questionId, CancellationToken cancellationToken = default)
        //{

        //    if (!_repositoryManager.QuestionCategoryRepository.IsCategoryExists(categoryId))
        //    {
        //        throw new QuestionCategoryNotFoundException(categoryId);
        //    }

        //    var question = await GetQuestinItemInCurrentDirectory(categoryId, questionId, cancellationToken);

        //    //Event (send messaga to RubbitMQ server)
        //    var eventMessage = _mapper.Map<QuestionItemDeleteEvent>(question);
        //    await _publishEndpoint.Publish(eventMessage);

        //    _repositoryManager.QuestionItemRepository.Remove(question);

        //    await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        //}

        //private async Task<QuestionItem> GetQuestinItemInCurrentDirectory(int categoryId, int questionId, CancellationToken cancellationToken = default)
        //{
        //    var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionId, cancellationToken);

        //    if (question is null)
        //    {
        //        throw new QuestionItemNotFoundException(questionId);
        //    }

        //    if (question.QuestionCategoryId != categoryId)
        //    {
        //        throw new QuestionItemDoesNotBelongToQuestionCategoryException(categoryId, questionId);
        //    }

        //    return question;
        //}

        //public async Task<QuestionItemReadDto> GetQuestionByIdIncludeAnswersAsync(int questionId, CancellationToken cancellationToken = default)
        //{
        //    var question = await _repositoryManager.QuestionItemRepository.GetQuestionByIdIncludeAnswersAsync(questionId, cancellationToken);

        //    if (question is null)
        //    {
        //        throw new QuestionItemNotFoundException(questionId);
        //    }

        //    var questionDto = _mapper.Map<QuestionItemReadDto>(question);

        //    return questionDto;
        //}

    }
}