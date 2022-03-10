using System;
using Grpc.Core;
using GrpcQuestion;
using System.Threading.Tasks;
using Question.Domain.Entities;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Question.API.Application.Services.Interfaces;

namespace Question.API.Grpc
{
    public class QuestionGrpcService : QuestionGrpc.QuestionGrpcBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<QuestionGrpcService> _logger;

        public QuestionGrpcService(IServiceManager serviceManager, ILogger<QuestionGrpcService> logger)
        {
            _serviceManager = serviceManager ?? throw new ArgumentNullException(nameof(serviceManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<QuestionUnitModel> GetQuestionUnitFromQuestionData(GetQuestionUnit request, ServerCallContext context)
        {
            // Get data from DB
            var questionUnit = await _serviceManager.QuestionItemService.GetByIdIncludeAnswersAsync(request.QuestionId);

            // Create response QuestionUnitModel
            var response = new QuestionUnitModel();

            response.Context = questionUnit.Context;
            response.AnswerKey = GetAnswerKeys(questionUnit.QuestionAnswers);
            response.TotalNumberAnswer = questionUnit.QuestionAnswers.Count;

            return await Task.FromResult(response);
        }

        private string GetAnswerKeys(ICollection<QuestionAnswer> answers)
        {
            string answerKeys = "";

            foreach (var item in answers)
            {
                if (item.IsCorrectAnswer == true)
                {
                    if(item.CharKey != "T")
                    {
                        answerKeys += item.CharKey;
                    }
                    else
                    {
                        answerKeys = item.Context;
                    }
                }
            }

            return answerKeys;
        }
    }
}