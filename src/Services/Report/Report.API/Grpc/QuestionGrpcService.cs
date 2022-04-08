using System;
using GrpcQuestion;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Report.API.Grpc
{
    public class QuestionGrpcService
    {
        private readonly ILogger<QuestionGrpcService> _logger;
        private readonly QuestionGrpc.QuestionGrpcClient _questionGrpcService;

        public QuestionGrpcService(QuestionGrpc.QuestionGrpcClient questionGrpcService, ILogger<QuestionGrpcService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _questionGrpcService = questionGrpcService ?? throw new ArgumentNullException(nameof(questionGrpcService));
        }

        public async Task<QuestionUnitModel> GetQuestionUnitFromQuestionData(int questionId)
        {
            var request = new GetQuestionUnit { QuestionId = questionId };

            return await _questionGrpcService.GetQuestionUnitFromQuestionDataAsync(request);

        }

    }
}