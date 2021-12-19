using Exam.Domain.Contracts.Dtos;
using Exam.Domain.Domain.Repositories;
using Exam.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using Exam.Domain.Domain.Exeptions;
using Exam.Domain.Domain.Entities;

namespace Exam.Domain.Services
{
    internal sealed class ExamService : IExamService
    {
        private readonly IRepositoryManager _repositoryManager;

        public ExamService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<ExamReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {   
            var exams = await _repositoryManager.ExamRepository.GetAllAsync(cancellationToken);
            var examsDto = exams.Adapt<IEnumerable<ExamReadDto>>();
            return examsDto;
        }

        public async Task<ExamReadDto> GetByIdAsync(int examId, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamRepository.GetByIdAsync(examId, cancellationToken);

            if(exam is null)
            {
                throw new ExamNotFoundException(examId);
            }

            var examDto = exam.Adapt<ExamReadDto>();

            return examDto;
        }

        public async Task<IEnumerable<ExamReadDto>> GetExamByCategoryAsync(string category, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamRepository.GetExamByCategoryAsync(category, cancellationToken);

            if (exam is null)
            {
                throw new ExamNotFoundException(category);
            }

            var examDto = exam.Adapt<IEnumerable<ExamReadDto>>();

            return examDto;
        }

        public async Task<IEnumerable<ExamReadDto>> GetExamByTitleAsync(string title, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamRepository.GetExamByTitleAsync(title, cancellationToken);

            if (exam is null)
            {
                throw new ExamNotFoundException(title);
            }

            var examDto = exam.Adapt<IEnumerable<ExamReadDto>>();

            return examDto;
        }

        public async Task UpdateAsync(int examId, ExamUpdateDto examUpdateDto, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamRepository.GetByIdAsync(examId, cancellationToken);

            if (exam is null)
            {
                throw new ExamNotFoundException(examId);
            }

            exam.Title = examUpdateDto.Title;
            exam.Category = examUpdateDto.Category;
            exam.Description = examUpdateDto.Description;
            exam.DurationTime = examUpdateDto.DurationTime;
            exam.PassingScore = examUpdateDto.PassingScore;
            exam.DateExam = examUpdateDto.DateExam;
            exam.Status = examUpdateDto.Status;

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ExamReadDto> CreateAsync(ExamCreateDto examCreateDto, CancellationToken cancellationToken = default)
        {
            var exam = examCreateDto.Adapt<ExamItem>();
            _repositoryManager.ExamRepository.Insert(exam);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
            return exam.Adapt<ExamReadDto>();
        }

        public async Task DeleteAsync(int examId, CancellationToken cancellationToken = default)
        {
            var exam = await _repositoryManager.ExamRepository.GetByIdAsync(examId, cancellationToken);

            if(exam is null)
            {
                throw new ExamNotFoundException(examId);
            }

            _repositoryManager.ExamRepository.Remove(exam);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
