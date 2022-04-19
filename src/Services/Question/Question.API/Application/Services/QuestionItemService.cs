using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Question.Domain.Entities;
using Question.Domain.Repositories;
using Question.API.Application.Exceptions;
using Question.API.Application.Services.Interfaces;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;

using AutoMapper;
using MassTransit;

using Exam.API.Application.IntegrationEvents.Events;
using Question.API.Grpc;
using GrpcExam;
using Question.API.Grpc.Interfaces;

namespace Question.API.Application.Services
{
    // Question service
    // Service in which the interface IQuestionItemService and its methods are implemented
    internal sealed class QuestionItemService : IQuestionItemService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IExamGrpcService _examGrpcService;
        private readonly IReportGrpcService _reportGrpcService;
        public QuestionItemService(IRepositoryManager repositoryManager, IMapper mapper,  IExamGrpcService examGrpcService, IReportGrpcService reportGrpcService)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _examGrpcService = examGrpcService;
            _reportGrpcService = reportGrpcService;
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

            var res =  CheckQuestion(questionId);
            if(res.Exists)
            {
                foreach (var item in res.Exams)
                {
                    var existsExamInReport =    _reportGrpcService.CheckIfExistsExamInReports(item);

                    if(existsExamInReport.Exists)
                    {
                        throw new BadRequestMessage($"Could not update question.The question with id: {questionId} already used in Report !");
                    }
                }
            }

            // TODO normal change answers
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

            var res =  CheckQuestion(questionId);
            if (res.Exists)
            {
                foreach (var item in res.Exams)
                {
                    var existsExamInReport =  _reportGrpcService.CheckIfExistsExamInReports(item);

                    if (existsExamInReport.Exists)
                    {
                        throw new BadRequestMessage($"Could not delete question.The question with id: {questionId} already used in Report !");
                    }
                }
            }

            _repositoryManager.QuestionItemRepository.Remove(question);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }


        /// <summary>
        /// Checks if questions exists in Report
        /// </summary>
        /// <param name="id">Id Question</param>
        /// <returns></returns>
        private ExamResponse CheckQuestion(int id)
        {
            var existsInExam =  _examGrpcService.CheckIfQuestionExistsInExam(id);

            return existsInExam;
            
        }
    }
}