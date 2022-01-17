using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Exam.Domain.Entities;
using Exam.Domain.Repositories;
using Exam.API.Application.Exceptions;
using Exam.API.Application.Services.Abstractions;
using Exam.API.Application.Contracts.ExamItemDtos;
using AutoMapper;

namespace Exam.API.Application.Services
{
    internal sealed class ExamItemService : IExamItemService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ExamItemService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
        }


        public async Task<IEnumerable<ExamItemReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var exams = await _repositoryManager.ExamItemRepository.GetAllAsync(cancellationToken);
            var examsDto = _mapper.Map<IEnumerable<ExamItemReadDto>>(exams);

            return examsDto;
        }

        public async Task<IEnumerable<ExamItemReadDto>> GetAllByStatusAsync(ExamStatus status, CancellationToken cancellationToken = default)
        {
            if (!Enum.IsDefined(typeof(ExamStatus), status))
            {
                throw new ArgumentNullException(nameof(status));
            }

            var exams = await _repositoryManager.ExamItemRepository.GetAllByStatusAsync(status, cancellationToken);

            if (!exams.Any())
            {
                throw new ExamNotFoundException(status.ToString());
            }

            var examsDto = _mapper.Map<IEnumerable<ExamItemReadDto>>(exams);

            return examsDto;
        }


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


        public async Task<ExamItemReadDto> CreateAsync(ExamItemCreateDto examCreateDto, CancellationToken cancellationToken = default)
        {
            var exam = _mapper.Map<ExamItem>(examCreateDto);

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
            exam.DateExam = examUpdateDto.DateExam;

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }


        public async Task DeleteAsync(int examId, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamItemRepository.GetByIdAsync(examId, cancellationToken);

            if (exam is null)
            {
                throw new ExamNotFoundException(examId);
            }

            _repositoryManager.ExamItemRepository.Remove(exam);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
