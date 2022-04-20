using Applicant.API.Grpc.Interfaces;
using Grpc.Net.Client;
using GrpcExam;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Applicant.API.Grpc
{
    public class ExamGrpcService : IExamGrpcService
    {
        private readonly ILogger<ExamGrpcService> _logger;
        private readonly IConfiguration _configuration;
        private GrpcChannel channel;
        private ExamGrpc.ExamGrpcClient client;

        public ExamGrpcService(ILogger<ExamGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            channel = GrpcChannel.ForAddress(_configuration["GrpcExamSettings:ExamUrl"]);
            client = new ExamGrpc.ExamGrpcClient(channel);
        }

        public ExamQuestionsResponse GetExamQuestions(int id)
        {
            Console.WriteLine($"---> calling Exam GRPC Service: {_configuration["GrpcExamSettings:ExamUrl"]}");

            try
            {
                var request = new GetExamItem() { ExamId = id };


                return client.GetExamQuestions(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }

        public ExamItemModel GetExamItem(int idExam)
        {
            Console.WriteLine($"---> calling Exam GRPC Service: {_configuration["GrpcExamSettings:ExamUrl"]}");

            try
            {
                var request = new GetExamItem() { ExamId = idExam };

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
