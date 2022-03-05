using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Question.Domain.Entities;
using Question.Domain.Repositories;
using Question.API.Application.Exceptions;
using Question.API.Application.Services.Interfaces;
using Question.API.Application.Contracts.Dtos.QuestionAnswerDtos;
using AutoMapper;

namespace Question.API.Application.Services
{
    // Question answer service. 
    // Service in which the interface IQuestionAnswerService and its methods are implemented

    internal sealed class QuestionAnswerService : IQuestionAnswerService
    {

        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public QuestionAnswerService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }



        // Get all answers from DB 
        public async Task<IEnumerable<QuestionAnswerReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var answers = await _repositoryManager.QuestionAnswerRepository
                .GetAllAsync(cancellationToken);

            var answersDto = _mapper.Map<IEnumerable<QuestionAnswerReadDto>>(answers);

            return answersDto;
        }


        // Get all answers by QuestionItem ID from DB
        public async Task<IEnumerable<QuestionAnswerReadDto>> GetAllByQuestionItemIdAsync(int questionId, CancellationToken cancellationToken = default)
        {
            var answers = await _repositoryManager.QuestionAnswerRepository
                .GetAllByQuestionItemIdAsync(questionId, cancellationToken);

            if(answers is null)
            {
                throw new QuestionAnswerNotFoundException(questionId);
            }

            var answersDto = _mapper.Map<IEnumerable<QuestionAnswerReadDto>>(answers);

            return answersDto;
        }


        // Get question by ID from DB
        public async Task<QuestionAnswerReadDto> GetByIdAsync(int answerId, CancellationToken cancellationToken = default)
        {
            var answer = await _repositoryManager.QuestionAnswerRepository
                .GetByIdAsync(answerId, cancellationToken);

            if (answer is null)
            {
                throw new QuestionAnswerNotFoundException(answerId);
            }

            var answerDto = _mapper.Map<QuestionAnswerReadDto>(answer);

            return answerDto;

        }


        // Create new answer
        public async Task<QuestionAnswerReadDto> CreateAsync(QuestionAnswerCreateDto answerCreateDto, CancellationToken cancellationToken = default)
        {

            if (answerCreateDto is null)
            {
                throw new QuestionAnswerArgumentException(nameof(answerCreateDto));
            }

            var question = await _repositoryManager.QuestionItemRepository
                .GetByIdAsync(answerCreateDto.QuestionItemId);

            if (question is null)
            {
                throw new QuestionAnswerDoesNotBelongToQuestionItemException(answerCreateDto.QuestionItemId);
            }

            var answers = (List<QuestionAnswer>)await _repositoryManager.QuestionAnswerRepository
                .GetAllByQuestionItemIdAsync(question.Id);

            CheckCorrectAnswerTypeWhenCreate(question, answerCreateDto, answers);

            var answer = _mapper.Map<QuestionAnswer>(answerCreateDto);
            answer.QuestionItemId = question.Id;

            _repositoryManager.QuestionAnswerRepository.Insert(answer);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<QuestionAnswerReadDto>(answer);

        }


        // Update current answer
        public async Task UpdateAsync(int answerId, QuestionAnswerUpdateDto answerUpdateDto, CancellationToken cancellationToken = default)
        {

            if (answerUpdateDto is null)
            {
                throw new QuestionAnswerArgumentException(nameof(answerUpdateDto));
            }

            var question = await _repositoryManager.QuestionItemRepository
                .GetByIdAsync(answerUpdateDto.QuestionItemId);

            if (question is null)
            {
                throw new QuestionAnswerDoesNotBelongToQuestionItemException(answerUpdateDto.QuestionItemId);
            }

            var answers = (List<QuestionAnswer>)await _repositoryManager.QuestionAnswerRepository
                .GetAllByQuestionItemIdAsync(question.Id);

            CheckCorrectAnswerTypeWhenUpdate(question, answerUpdateDto, answers);
           
            var answer = await _repositoryManager.QuestionAnswerRepository.GetByIdAsync(answerId, cancellationToken);

            answer.Context = answerUpdateDto.Context;
            answer.IsCorrectAnswer = answerUpdateDto.IsCorrectAnswer;

            await _repositoryManager.UnitOfWork.SaveChangesAsync();
        }


        // Delete current answer
        public async Task DeleteAsync(int answerId, CancellationToken cancellationToken = default)
        {

            var answer = await _repositoryManager.QuestionAnswerRepository.GetByIdAsync(answerId);

            if (answer is null)
            {
                throw new QuestionAnswerNotFoundException(answerId);
            }

            _repositoryManager.QuestionAnswerRepository.Remove(answer);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }


        private void CheckCorrectAnswerTypeWhenCreate(QuestionItem question, QuestionAnswerCreateDto questionAnswerCreateDto, List<QuestionAnswer> answers)
        {

            string[] CorrectCharKeys = { "A", "B", "C", "D", "E" };

            var isCharKeyExist = CorrectCharKeys.Contains(questionAnswerCreateDto.CharKey);


            switch (question.AnswerType)
            {
                case AnswerType.Text:
                    {
                        // Check for char key exist. Char key must be only [T]
                        if (questionAnswerCreateDto.CharKey != "T")
                        {
                            throw new QuestionAnswerArgumentException($"For questions with answer type {question.AnswerType}, char key must be only T!");
                        }

                        // Check for count answer. They mustn't exceed 1 answer
                        if (answers.Count >= 1)
                        {
                            throw new QuestionAnswerQuantityLimitException(1);
                        }

                        // Check for correct answer. They must be only one correct answer
                        if (questionAnswerCreateDto.IsCorrectAnswer != true)
                        {
                            throw new QuestionAnswerArgumentException($"For questions with answer type {question.AnswerType}, correct answer must be only TRUE!");
                        }
                    }
                    break;

                case AnswerType.Single:
                    {
                        // Check for char key exist. Char key must be only [A,B,C,D,E]
                        if (!isCharKeyExist)
                        {
                            throw new QuestionAnswerArgumentException($"For questions with answer type {question.AnswerType}, char key must be only A or B or C or D or E");
                        }

                        // Check for count answer. They mustn't exceed 5 answers 
                        if (answers.Count > 5)
                        {
                            throw new QuestionAnswerQuantityLimitException(5);
                        }

                        // Finding count correct answers
                        var countCorrectAnswer = answers.Where(k => k.IsCorrectAnswer == true).ToList();

                        // Check for count correct answer.
                        if (countCorrectAnswer.Count >= 1 && questionAnswerCreateDto.IsCorrectAnswer == true)
                        {
                            throw new QuestionAnswerArgumentException($"For questions with answer type {question.AnswerType}, count correct answer must be only ONE");
                        }

                        // Finding char key
                        var isCharKeyExistInDataBase = answers.FirstOrDefault(k => k.CharKey == questionAnswerCreateDto.CharKey);

                        // Check is exist current key in questions
                        if (isCharKeyExistInDataBase != null)
                        {
                            throw new QuestionAnswerFieldException(questionAnswerCreateDto.CharKey);
                        }
                    }
                    break;

                case AnswerType.Multiple:
                    {
                        // Check for char key exist. Char key must be only [A,B,C,D,E]
                        if (!isCharKeyExist)
                        {
                            throw new QuestionAnswerArgumentException($"For questions with answer type {question.AnswerType}, char key must be only A or B or C or D or E");
                        }

                        // Check for count answer. They mustn't exceed 5 answers
                        if (answers.Count > 5)
                        {
                            throw new QuestionAnswerQuantityLimitException(5);
                        }

                        // Finding char key
                        var isExistCharKey = answers.FirstOrDefault(k => k.CharKey == questionAnswerCreateDto.CharKey);

                        // Check is exist current key in questions
                        if (isExistCharKey != null)
                        {
                            throw new QuestionAnswerFieldException(questionAnswerCreateDto.CharKey);
                        }
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"Attention! Answer type - {question.AnswerType} is not correct.");
            }
        }

        private void CheckCorrectAnswerTypeWhenUpdate(QuestionItem question, QuestionAnswerUpdateDto answerUpdateDto, List<QuestionAnswer> answers)
        {
            if (question.AnswerType == AnswerType.Text)
            {
                // Check for correct answer. They must be only one correct answer
                if (answerUpdateDto.IsCorrectAnswer != true)
                {
                    throw new QuestionAnswerArgumentException($"For questions with answer type {question.AnswerType}, correct answer must be only TRUE!");
                }
            }

            if (question.AnswerType == AnswerType.Single)
            {
                // Finding count correct answers
                var countCorrectAnswer = answers.Where(k => k.IsCorrectAnswer == true).ToList();

                // Check for count correct answer.
                if (countCorrectAnswer.Count >= 1 && answerUpdateDto.IsCorrectAnswer == true)
                {
                    throw new QuestionAnswerArgumentException($"For questions with answer type {question.AnswerType}, count correct answer must be only ONE");
                }
            }
        }

    }
}