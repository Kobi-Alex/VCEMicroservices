using System;
using GrpcQuestion;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Report.API.Grpc.Interfaces;
using Microsoft.Extensions.Configuration;
using Grpc.Net.Client;

namespace Report.API.Grpc
{
    public class QuestionGrpcService : IQuestionGrpcService
    {
        private readonly ILogger<QuestionGrpcService> _logger;
        private readonly IConfiguration _configuration;

        public QuestionGrpcService(ILogger<QuestionGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }

        public QuestionUnitModel GetQuestionUnitFromQuestionData(int questionId)
        {
            Console.WriteLine($"---> Calling Question GRPC Service: {_configuration["GrpcQuestionSettings:QuestionUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcQuestionSettings:QuestionUrl"]);
            var client = new QuestionGrpc.QuestionGrpcClient(channel);

            try
            {
                var request = new GetQuestionUnit { QuestionId = questionId };

                return client.GetQuestionUnitFromQuestionData(request);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }
    }


    //public class QuestionGrpcService
    //{
    //    private readonly ILogger<QuestionGrpcService> _logger;
    //    private readonly QuestionGrpc.QuestionGrpcClient _questionGrpcService;

    //    public QuestionGrpcService(QuestionGrpc.QuestionGrpcClient questionGrpcService, ILogger<QuestionGrpcService> logger)
    //    {
    //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //        _questionGrpcService = questionGrpcService ?? throw new ArgumentNullException(nameof(questionGrpcService));
    //    }

    //    public async Task<QuestionUnitModel> GetQuestionUnitFromQuestionData(int questionId)
    //    {
    //        var request = new GetQuestionUnit { QuestionId = questionId };

    //        return await _questionGrpcService.GetQuestionUnitFromQuestionDataAsync(request);

    //    }

    //}
}