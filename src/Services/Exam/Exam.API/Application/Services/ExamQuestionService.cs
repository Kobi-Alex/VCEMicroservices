using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Exam.Domain.Entities;
using Exam.Domain.Repositories;
using Exam.API.Application.Exceptions;
using Exam.API.Application.Services.Interfaces;
using Exam.API.Application.Contracts.ExamQuestionDtos;

using AutoMapper;
using Exam.API.Grpc;

namespace Exam.API.Application.Services
{
    internal sealed class ExamQuestionService : IExamQuestionService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ReportGrpcService _reportGrpcService;
        public ExamQuestionService(IRepositoryManager repositoryManager, IMapper mapper, ReportGrpcService reportGrpcService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _reportGrpcService = reportGrpcService;
        }

        public async Task<IEnumerable<ExamQuestionReadDto>> GetAllByExamItemIdAsync(int examId, CancellationToken cancellationToken = default)
        {
            if (!_repositoryManager.ExamItemRepository.IsExamItemExists(examId))
            {
                throw new ExamNotFoundException(examId);
            }

            var questions = await _repositoryManager.ExamQuestionRepository.GetAllByExamItemAsync(examId, cancellationToken);
            var questionsDto = _mapper.Map<IEnumerable<ExamQuestionReadDto>>(questions);

            return questionsDto;
        }

        public async Task<ExamQuestionReadDto> GetByIdAsync(int examId, int questionId, CancellationToken cancellationToken)
        {
            var exam = await _repositoryManager.ExamItemRepository.GetByIdAsync(examId, cancellationToken);

            if (exam is null)
            {
                throw new ExamNotFoundException(examId);
            }

            var question = await _repositoryManager.ExamQuestionRepository.GetByIdAsync(questionId, cancellationToken);

            if (question is null)
            {
                throw new QuestionNotFoundException(questionId);
            }

            if (question.ExamItemId != exam.Id)
            {
                throw new QuestionDoesNotBelongToExamException(exam.Id, question.Id);
            }

            var questionDto = _mapper.Map<ExamQuestionReadDto>(question);

            return questionDto;
        }

        public async Task<ExamQuestionReadDto> CreateAsync(int examId, ExamQuestionCreateDto examQuestionCreateDto, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamItemRepository.GetByIdAsync(examId, cancellationToken);
           
            if (exam is null)
            {
                throw new ExamNotFoundException(examId);
            }

            var question = _mapper.Map<ExamQuestion>(examQuestionCreateDto);
            question.ExamItemId = exam.Id;

            await CheckExam(examId);

            _repositoryManager.ExamQuestionRepository.Insert(question);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ExamQuestionReadDto>(question);
        }

        public async Task DeleteAsync(int examId, int questionId, CancellationToken cancellationToken = default)
        {

            var exam = await _repositoryManager.ExamItemRepository.GetByIdAsync(examId, cancellationToken);

            if (exam is null)
            {
                throw new ExamNotFoundException(examId);
            }

            var question = await _repositoryManager.ExamQuestionRepository.GetByIdAsync(questionId, cancellationToken);

            if (question is null)
            {
                throw new QuestionNotFoundException(questionId);
            }

            if (question.ExamItemId != exam.Id)
            {
                throw new QuestionDoesNotBelongToExamException(exam.Id, question.Id);
            }

            await CheckExam(examId);
            _repositoryManager.ExamQuestionRepository.Remove(question);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Checks if exam exists in Report
        /// </summary>
        /// <param name="id">Id Exam</param>
        /// <returns></returns>
        private async Task CheckExam(int id)
        {
            var res = await _reportGrpcService.CheckIfExistsExamInReports(id);

            if (res.Exists)
            {
                throw new BadRequestMessage($"Could not change exam! This exam with id: {id} already used!");
            }
        }

    }
}
