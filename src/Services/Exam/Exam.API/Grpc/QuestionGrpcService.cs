using Exam.API.Grpc.Interfaces;
using Grpc.Net.Client;
using GrpcQuestion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.API.Grpc
{
    public class QuestionGrpcService: IQuestionGrpcService
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

        public QuestionUnitModel GetQuestionById(int id)
        {
            Console.WriteLine($"---> Calling Question GRPC Service: {_configuration["GrpcQuestionSettings:QuestionUrl"]}");

            try
            {
                var request = new GetQuestionUnit() { QuestionId = id };

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
