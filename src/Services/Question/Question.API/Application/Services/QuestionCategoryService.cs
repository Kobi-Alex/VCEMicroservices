using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Question.Domain.Entities;
using Question.Domain.Repositories;
using Question.API.Application.Exceptions;
using Question.API.Application.Contracts.Dtos.QuestionCategoryDtos;
using Question.API.Application.Services.Interfaces;
using AutoMapper;

namespace Question.API.Application.Services
{
    // Category service
    // Service in which the interface IQuestionCategoryService and its methods are implemented
    internal sealed class QuestionCategoryService : IQuestionCategoryService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public QuestionCategoryService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
        }


        // Get all categories from DB
        public async Task<IEnumerable<QuestionCategoryReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _repositoryManager.QuestionCategoryRepository.GetAllAsync(cancellationToken);
            var categoriesDto = _mapper.Map<IEnumerable<QuestionCategoryReadDto>>(categories);

            return categoriesDto;
        }


        // Get question by ID from DB
        public async Task<QuestionCategoryReadDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await _repositoryManager.QuestionCategoryRepository.GetByIdAsync(id, cancellationToken);

            if (category is null)
            {
                throw new QuestionCategoryNotFoundException(id);
            }

            var categoryDto = _mapper.Map<QuestionCategoryReadDto>(category);

            return categoryDto;
        }


        // Create new category
        public async Task<QuestionCategoryReadDto> CreateAsync(QuestionCategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default)
        {
            if (categoryCreateDto is null)
            {
                throw new QuestionCategoryArgumentException(nameof(categoryCreateDto));
            }

            var category = _mapper.Map<QuestionCategory>(categoryCreateDto);

            _repositoryManager.QuestionCategoryRepository.Insert(category);

            await _repositoryManager.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<QuestionCategoryReadDto>(category);
        }


        // Update current category
        public async Task UpdateAsync(int id, QuestionCategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default)
        {
            if (categoryUpdateDto is null)
            {
                throw new QuestionCategoryArgumentException(nameof(categoryUpdateDto));
            }

            var category = await _repositoryManager.QuestionCategoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                throw new QuestionCategoryNotFoundException(id);
            }

            category.Name = categoryUpdateDto.Name;

            await _repositoryManager.UnitOfWork.SaveChangesAsync();
        }
    }
}