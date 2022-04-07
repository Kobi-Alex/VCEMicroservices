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
using Question.API.Grpc;
using GrpcExam;

namespace Question.API.Application.Services
{
    // Question answer service. 
    // Service in which the interface IQuestionAnswerService and its methods are implemented

    internal sealed class QuestionAnswerService : IQuestionAnswerService
    {

        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ExamGrpcService _examGrpcService;
        private readonly ReportGrpcService _reportGrpcService; 

        public QuestionAnswerService(IRepositoryManager repositoryManager, IMapper mapper, ExamGrpcService examGrpcService, ReportGrpcService reportGrpcService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _examGrpcService = examGrpcService;
            _reportGrpcService = reportGrpcService;
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

            CheckCorrectAnswerType(question, answerCreateDto, answers);

            var answer = _mapper.Map<QuestionAnswer>(answerCreateDto);
            answer.QuestionItemId = question.Id;


            var res = await CheckQuestion(question.Id);

            if (res.Exists)
            {
                foreach (var item in res.Exams)
                {
                    var existsExamInReport = await _reportGrpcService.CheckIfExistsExamInReports(item);

                    if (existsExamInReport.Exists)
                    {
                        throw new BadRequestMessage($"Could not add a new answer to question.The question with id: {question.Id} already used in Report !");
                    }
                }
            }

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

           var res = await CheckQuestion(question.Id);

            if(res.Exists)
            {
                foreach (var item in res.Exams)
                {
                    var existsExamInReport = await _reportGrpcService.CheckIfExistsExamInReports(item);

                    if (existsExamInReport.Exists)
                    {
                        throw new BadRequestMessage($"Could not update answer in question.The question with id: {question.Id} already used in Report !");
                    }
                }
            }

            var answers = (List<QuestionAnswer>)await _repositoryManager.QuestionAnswerRepository
                .GetAllByQuestionItemIdAsync(question.Id);

            var answer = await _repositoryManager.QuestionAnswerRepository.GetByIdAsync(answerId, cancellationToken);

            CheckCorrectAnswerType(question, answerUpdateDto, answers, answer);

            answer.Context = answerUpdateDto.Context;
            answer.IsCorrectAnswer = answerUpdateDto.IsCorrectAnswer;
            answer.CharKey = answerUpdateDto.CharKey;

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

            var question = await _repositoryManager.QuestionItemRepository
                .GetByIdAsync(answer.QuestionItemId);

            if (question != null)
            {
                var res = await CheckQuestion(question.Id);

                if (res.Exists)
                {
                    foreach (var item in res.Exams)
                    {
                        var existsExamInReport = await _reportGrpcService.CheckIfExistsExamInReports(item);

                        if (existsExamInReport.Exists)
                        {
                            throw new BadRequestMessage($"Could delete answer in question.The question with id: {question.Id} already used in Report !");
                        }
                    }
                }
            }

            

            _repositoryManager.QuestionAnswerRepository.Remove(answer);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }


        /// <summary>
        /// Checking correct answer type
        /// </summary>
        /// <param name="question"></param>
        /// <param name="questionAnswerCreateDto"></param>
        /// <param name="answers"></param>        
        private void CheckCorrectAnswerType(QuestionItem question, QuestionAnswerCreateDto questionAnswerCreateDto, List<QuestionAnswer> answers)
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

                        // Checking if exist current key in questions
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

        /// <summary>
        /// Checks if questions exists in Report
        /// </summary>
        /// <param name="id">Id Question</param>
        /// <returns></returns>
        private async Task<ExamResponse> CheckQuestion(int id)
        {
            var existsInExam = await _examGrpcService.CheckIfQuestionExistsInExam(id);

            return existsInExam;
        }

        /// <summary>
        /// Checking correct answer type
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answerUpdateDto"></param>
        /// <param name="answers"></param>
        /// <param name="currentAnswer"></param>
        private void CheckCorrectAnswerType(QuestionItem question, QuestionAnswerUpdateDto answerUpdateDto, 
            List<QuestionAnswer> answers, QuestionAnswer currentAnswer)
        {
            if (question.AnswerType == AnswerType.Text)
            {
                // Check for correct answer. They must be only one correct answer
                if (answerUpdateDto.IsCorrectAnswer == false)
                {
                    throw new QuestionAnswerArgumentException($"For questions with answer type {question.AnswerType}, correct answer must be only TRUE!");
                }
            }

            if (question.AnswerType == AnswerType.Single)
            {
                // Check update and current answer 
                if(answerUpdateDto.IsCorrectAnswer != currentAnswer.IsCorrectAnswer)
                {
                    // If the updated answer equal true then other answers equal false
                    // and on the contrary
                    if (answerUpdateDto.IsCorrectAnswer == true)
                    {
                        foreach (var item in answers)
                        {
                            item.IsCorrectAnswer = false;
                        }
                    }
                    else
                    {
                        foreach (var item in answers)
                        {
                            if(item.Id != currentAnswer.Id)
                            {
                                item.IsCorrectAnswer = true;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}