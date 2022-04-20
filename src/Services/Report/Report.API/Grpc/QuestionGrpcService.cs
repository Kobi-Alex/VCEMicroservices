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
        private GrpcChannel channel;
        private QuestionGrpc.QuestionGrpcClient client;

        public QuestionGrpcService(ILogger<QuestionGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
            channel = GrpcChannel.ForAddress(_configuration["GrpcQuestionSettings:QuestionUrl"]);
            client = new QuestionGrpc.QuestionGrpcClient(channel);
        }

        public QuestionUnitModel GetQuestionUnitFromQuestionData(int questionId)
        {
            Console.WriteLine($"---> Calling Question GRPC Service: {_configuration["GrpcQuestionSettings:QuestionUrl"]}");

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
}