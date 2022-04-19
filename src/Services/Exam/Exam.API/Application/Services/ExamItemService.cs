using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Exam.Domain.Entities;
using Exam.Domain.Repositories;
using Exam.API.Application.Exceptions;
using Exam.API.Application.Services.Interfaces;
using Exam.API.Application.Contracts.ExamItemDtos;

using AutoMapper;
using Exam.API.Grpc;
using System.Linq;
using Exam.API.Grpc.Interfaces;

namespace Exam.API.Application.Services
{
    internal sealed class ExamItemService : IExamItemService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IReportGrpcService _reportGrpcService;
        private readonly IApplicantGrpcService _applicantGprcService;

        public ExamItemService(IRepositoryManager repositoryManager, IMapper mapper, IReportGrpcService reportGrpcService, IApplicantGrpcService applicantGprcService)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _reportGrpcService = reportGrpcService;
            _applicantGprcService = applicantGprcService;
        }

        public async Task<IEnumerable<ExamItemReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var exams = await _repositoryManager.ExamItemRepository.GetAllAsync(cancellationToken);
            var examsDto = _mapper.Map<IEnumerable<ExamItemReadDto>>(exams);

            return examsDto;
        }


        //public async Task<IEnumerable<ExamItemReadDto>> GetAllByStatusAsync(ExamStatus status, CancellationToken cancellationToken = default)
        //{
        //    if (!Enum.IsDefined(typeof(ExamStatus), status))
        //    {
        //        throw new ArgumentNullException(nameof(status));
        //    }

        //    var exams = await _repositoryManager.ExamItemRepository.GetAllByStatusAsync(status, cancellationToken);

        //    if (!exams.Any())
        //    {
        //        throw new ExamNotFoundException(status.ToString());
        //    }

        //    var examsDto = _mapper.Map<IEnumerable<ExamItemReadDto>>(exams);

        //    return examsDto;
        //}

        public async Task<ExamItemReadDto> GetByIdAsync(int examId, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamItemRepository.GetByIdAsync(examId, cancellationToken);

            if (exam is null)
            {
                throw new ExamNotFoundException(examId);
            }

            var examDto = _mapper.Map<ExamItemReadDto>(exam);

            return examDto;
        }

        public async Task<ExamItemReadDto> GetByIdIncludeExamQuestionsAsync(int examId, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamItemRepository.GetByIdIncludeExamQustionsAsync(examId, cancellationToken);

            if (exam is null)
            {
                // TODO check gRPC test if exam is exception !!!!!
                throw new ExamNotFoundException(examId);
            }

            var examDto = _mapper.Map<ExamItemReadDto>(exam);

            return examDto;
        }

        public async Task<ExamItemReadDto> CreateAsync(ExamItemCreateDto examCreateDto, CancellationToken cancellationToken = default)
        {
            var exam = _mapper.Map<ExamItem>(examCreateDto);

            // When creating exam, exam status equal not available
            //exam.Status = ExamStatus.NotAvailable;

            _repositoryManager.ExamItemRepository.Insert(exam);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ExamItemReadDto>(exam);
        }

        public async Task UpdateAsync(int examId, ExamItemUpdateDto examUpdateDto, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamItemRepository.GetByIdAsync(examId, cancellationToken);

            if (exam is null)
            {
                throw new ExamNotFoundException(examId);
            }

            exam.Title = examUpdateDto.Title;
            exam.Description = examUpdateDto.Description;
            exam.DurationTime = examUpdateDto.DurationTime;
            exam.PassingScore = examUpdateDto.PassingScore;


            if (CheckExam(examId))
            {
                throw new BadRequestMessage($"Could not update exam! This exam with id: {examId} already used in Report!");
            }


            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int examId, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamItemRepository.GetByIdAsync(examId, cancellationToken);

            if (exam is null)
            {
                throw new ExamNotFoundException(examId);
            }

            if (CheckExam(examId))
            {
                throw new BadRequestMessage($"Could not delete exam! This exam with id: {examId} already used in Report!");
            }

            var existsExamInUsers = _applicantGprcService.CheckIfExamExistsInUsers(examId);

            if (existsExamInUsers.Exists)
            {
                throw new BadRequestMessage($"Could not delete exam! This exam with id: {examId} already used in Users");
            }


            _repositoryManager.ExamItemRepository.Remove(exam);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<ExamItemReadDto>> GetAllByQuestionId(int questionId, CancellationToken cancellationToken = default)
        {
            var exams = await _repositoryManager.ExamItemRepository.FindAll(x => x.ExamQuestions.Where(x => x.QuestionItemId == questionId).Any());


            return _mapper.Map<IEnumerable<ExamItemReadDto>>(exams);
        }

        /// <summary>
        /// Checks if exam exists in Report
        /// </summary>
        /// <param name="id">Id Exam</param>
        /// <returns></returns>
        private bool CheckExam(int id)
        {
            var res = _reportGrpcService.CheckIfExistsExamInReports(id);

            return res.Exists;
        }
    }
}