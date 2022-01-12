using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Exam.API.Application.Services.Abstractions;
using Exam.Domain.Repositories;
using Exam.API.Application.Contracts.ExamQuestionDtos;

namespace Exam.API.Application.Services
{
    internal sealed class ExamQuestionService : IExamQuestionService
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;

        public ExamQuestionService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            this.repositoryManager = repositoryManager;
            this.mapper = mapper;
        }

        public Task<ExamQuestionReadDto> CreateAsync(int examId, ExamQuestionCreateDto examQuestionCreateDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int examId, int questionId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExamQuestionReadDto>> GetAllByExamItemIdAsync(int examId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ExamQuestionReadDto> GetByIdAsync(int examId, int questionId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
