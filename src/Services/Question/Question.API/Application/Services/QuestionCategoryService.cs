using AutoMapper;
using Question.API.Application.Contracts.Dtos.QuestionCategoryDtos;
using Question.API.Application.Services.Interfaces;
using Question.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Question.API.Application.Services
{
    internal sealed class QuestionCategoryService : IQuestionCategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public QuestionCategoryService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuestionCategoryReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _repositoryManager.QuestionCategoryRepository.GetAllAsync(cancellationToken);
            var categoriesDto = _mapper.Map<IEnumerable<QuestionCategoryReadDto>>(categories);

            return categoriesDto;
        }

        public Task<QuestionCategoryReadDto> GetByIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<QuestionCategoryReadDto> CreateAsync(QuestionCategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int categoryId, QuestionCategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}