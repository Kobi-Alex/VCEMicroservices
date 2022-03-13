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
            //var questionUnitRequest = new GetQuestionUnit { QuestionId = questionId };
            //return await _questionProtoService.GetQuestionUnitFromQuestionDataAsync(questionUnitRequest);

            //Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcSettings:QuestionUrl"]}");
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");

            //var client = new QuestionGrpc.QuestionGrpcClient(channel);

            //return await client.GetQuestionUnitFromQuestionDataAsync(request);

            var request = new GetQuestionUnit { QuestionId = questionId };

            return await _questionGrpcService.GetQuestionUnitFromQuestionDataAsync(request);

        }

    }
}