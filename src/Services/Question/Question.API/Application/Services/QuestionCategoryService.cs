using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Question.Domain.Repositories;
using Question.API.Application.Exceptions;
using Question.API.Application.Contracts.Dtos.QuestionCategoryDtos;
using Question.API.Application.Services.Interfaces;
using AutoMapper;
using Question.Domain.Entities;

namespace Question.API.Application.Services
{
    internal sealed class QuestionCategoryService : IQuestionCategoryService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public QuestionCategoryService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
        }



        public async Task<IEnumerable<QuestionCategoryReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _repositoryManager.QuestionCategoryRepository.GetAllAsync(cancellationToken);
            var categoriesDto = _mapper.Map<IEnumerable<QuestionCategoryReadDto>>(categories);

            return categoriesDto;
        }

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

        public async Task<QuestionCategoryReadDto> CreateAsync(QuestionCategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default)
        {
            if (categoryCreateDto is null)
            {
                throw new ArgumentNullException(nameof(categoryCreateDto));
            }

            var category = _mapper.Map<QuestionCategory>(categoryCreateDto);

            _repositoryManager.QuestionCategoryRepository.Insert(category);

            await _repositoryManager.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<QuestionCategoryReadDto>(category);
        }

        public async Task UpdateAsync(int id, QuestionCategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default)
        {
            var category = await _repositoryManager.QuestionCategoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                throw new QuestionCategoryNotFoundException(id);
            }

            category.Name = categoryUpdateDto.Name;

            await _repositoryManager.UnitOfWork.SaveChangesAsync();
        }



        //public async Task<QuestionItemReadDto> GetByIdAsync(int questionItemId, CancellationToken cancellationToken = default)
        //{
        //    var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionItemId, cancellationToken);

        //    if (question is null)
        //    {
        //        throw new QuestionItemNotFoundException(questionItemId);
        //    }

        //    var questionDto = _mapper.Map<QuestionItemReadDto>(question);

        //    return questionDto;
        //}


        //public async Task<QuestionItemReadDto> CreateAsync(QuestionItemCreateDto questionCreateDto, CancellationToken cancellationToken = default)
        //{
        //    if (questionCreateDto is null)
        //    {
        //        throw new ArgumentNullException(nameof(questionCreateDto));
        //    }

        //    var question = _mapper.Map<QuestionItem>(questionCreateDto);

        //    _repositoryManager.QuestionItemRepository.Insert(question);

        //    await _repositoryManager.UnitOfWork.SaveChangesAsync();

        //    return _mapper.Map<QuestionItemReadDto>(question);
        //}


        //public async Task UpdateAsync(int questionItemId, QuestionItemUpdateDto questionUpdateDto, CancellationToken cancellationToken = default)
        //{
        //    var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionItemId);

        //    if (question is null)
        //    {
        //        throw new QuestionItemNotFoundException(questionItemId);
        //    }


        //    question.Context = questionUpdateDto.Context;
        //    question.ReleaseDate = questionUpdateDto.ReleaseDate;

        //    await _repositoryManager.UnitOfWork.SaveChangesAsync();
        //}

        //public async Task DeleteAsync(int questionId, CancellationToken cancellationToken = default)
        //{
        //    var question = await _repositoryManager.QuestionItemRepository.GetByIdAsync(questionId);

        //    if (question is null)
        //    {
        //        throw new QuestionItemNotFoundException(questionId);
        //    }

        //    _repositoryManager.QuestionItemRepository.Remove(question);

        //    await _repositoryManager.UnitOfWork.SaveChangesAsync();
        //}
    }
}