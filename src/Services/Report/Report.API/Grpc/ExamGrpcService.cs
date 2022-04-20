using System;
using GrpcExam;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Report.API.Grpc.Interfaces;
using Microsoft.Extensions.Configuration;
using Grpc.Net.Client;

namespace Report.API.Grpc
{

    public class ExamGrpcService : IExamGrpcService
    {
        private readonly ILogger<ExamGrpcService> _logger;
        private readonly IConfiguration _configuration;
        private GrpcChannel channel;
        private ExamGrpc.ExamGrpcClient client;

        public ExamGrpcService(ILogger<ExamGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
             channel = GrpcChannel.ForAddress(_configuration["GrpcExamSettings:ExamUrl"]);
             client = new ExamGrpc.ExamGrpcClient(channel);
        }

        public ExamResponse CheckIfQuestionExistsInExam(int questionId)
        {
            Console.WriteLine($"---> Calling Exam GRPC Service: {_configuration["GrpcExamSettings:ExamUrl"]}");

            try
            {
                var request = new ExamRequest { QuestionId = questionId };

                return client.CheckIfQuestionExistsInExam(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }

        public ExamItemModel GetExamItemFromExamData(int examId)
        {
            Console.WriteLine($"---> Calling Exam GRPC Service: {_configuration["GrpcExamSettings:ExamUrl"]}");

            try
            {
                var request = new GetExamItem { ExamId = examId };

                return client.GetExamItemFromExamData(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }
    }
}