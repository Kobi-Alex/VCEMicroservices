using AutoMapper;
using Question.Domain.Contracts.Dtos;
using Question.Domain.Domain.Entities;
using Question.Domain.Domain.Exceptions;
using Question.Domain.Domain.Repositories;
using Question.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Question.Domain.Services
{
    internal sealed class QuestionService : IQuestionService
    {
        private IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public QuestionService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuestionReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var questions = await _repositoryManager.QuestionRepository.GetAllAsync(cancellationToken);
            var questionDto = _mapper.Map<IEnumerable<QuestionReadDto>>(questions);

            return questionDto;
        }

        public async Task<QuestionReadDto> GetByIdAsync(int questionId, CancellationToken cancellationToken = default)
        {
            var question = await _repositoryManager.QuestionRepository.GetByIdAsync(questionId, cancellationToken);
            var questionDto = _mapper.Map<QuestionReadDto>(question);

            return questionDto;
        }

        public async Task<IEnumerable<QuestionReadDto>> GetQuestionByCategoryAsync(string category, CancellationToken cancellationToken = default)
        {
            var questions = await _repositoryManager.QuestionRepository.GetQuestionByCategory(category, cancellationToken);

            if(questions is null)
            {
                throw new QuestionNotFoundException(category);
            }

            var questionDto = _mapper.Map<IEnumerable<QuestionReadDto>>(questions);

            return questionDto;
        }


        public async Task<QuestionReadDto> CreateAsync(QuestionCreateDto questionCreateDto, CancellationToken cancellationToken = default)
        {
            var question = _mapper.Map<QuestionItem>(questionCreateDto);
            _repositoryManager.QuestionRepository.Insert(question);
            await _repositoryManager.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<QuestionReadDto>(question);
        }

        public async Task UpdateAsync(int questionId, QuestionUpdateDto questionUpdateDto, CancellationToken cancellationToken = default)
        {
            var question = await _repositoryManager.QuestionRepository.GetByIdAsync(questionId);

            if(question is null)
            {
                throw new QuestionNotFoundException(questionId);
            }

            question.Category = questionUpdateDto.Category;
            question.Question = questionUpdateDto.Question;
            question.Answer = questionUpdateDto.Answer;

            question.QuestionOptionA = questionUpdateDto.QuestionOptionA;
            question.QuestionOptionB = questionUpdateDto.QuestionOptionB;
            question.QuestionOptionC = questionUpdateDto.QuestionOptionC;
            question.QuestionOptionD = questionUpdateDto.QuestionOptionD;

            question.AnswerOptionA = questionUpdateDto.AnswerOptionA;
            question.AnswerOptionB = questionUpdateDto.AnswerOptionB;
            question.AnswerOptionC = questionUpdateDto.AnswerOptionC;
            question.AnswerOptionD = questionUpdateDto.AnswerOptionD;

            await _repositoryManager.UnitOfWork.SaveChangesAsync();

        }

        public async Task DeleteAsync(int questionId, CancellationToken cancellationToken = default)
        {
            var question = await _repositoryManager.QuestionRepository.GetByIdAsync(questionId);

            if (question is null)
            {
                throw new QuestionNotFoundException(questionId);
            }

            _repositoryManager.QuestionRepository.Remove(question);

            await _repositoryManager.UnitOfWork.SaveChangesAsync();
        }
    }
}
